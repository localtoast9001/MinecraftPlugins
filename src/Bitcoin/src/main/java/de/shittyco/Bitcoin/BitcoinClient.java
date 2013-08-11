package de.shittyco.Bitcoin;

import java.io.IOException;
import java.io.OutputStream;
import java.net.Authenticator;
import java.net.HttpURLConnection;
import java.net.URL;

import org.codehaus.jackson.JsonFactory;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

/**
 * Manages communication to a bitcoin node.
 * @author jrowlett
 */
public class BitcoinClient {
    /**
     * Authenticator to use for requests.
     */
    private BitcoinAuthenticator auth;

    /**
     * URL of the JSON-RPC end point.
     */
    private URL url;

    /**
     * The factory to serialize JSON.
     */
    private JsonFactory factory = new JsonFactory();

    /**
     * Initializes a new instance of the BitcoinClient class.
     * @param urlValue the URL to connect to
     * @param userName the user name
     * @param password the password
     */
    public BitcoinClient(
        final URL urlValue,
        final String userName,
        final String password) {
        this.auth = new BitcoinAuthenticator(userName, password);
        this.url = urlValue;
    }

    /**
     * Gets information about the server.
     * @return - info structure.
     * @throws ServerErrorException - when there is a server error.
     */
    public final BitcoinInfo getInfo() throws ServerErrorException {
        JsonParser parser = this.call("getinfo", new Object[] {});
        return parseBitcoinInfo(parser);
    }

    /**
     * Gets the balance of the account.
     * @param account - the account name
     * @return - the balance in BTC
     * @throws ServerErrorException - when there is a server error.
     */
    public final BTC getBalance(final String account)
        throws ServerErrorException {
        Object[] args = null;
        if (account == null || account.isEmpty()) {
            args = new Object[0];
        } else {
            args = new Object[] {account};
        }

        JsonParser parser = this.call("getbalance", args);
        return parseBalance(parser);
    }

    /**
     * Gets the address for the account.
     * @param account - the account name
     * @return - the address string
     * @throws ServerErrorException - when there is a server error.
     */
    public final String getAccountAddress(final String account)
        throws ServerErrorException {
        JsonParser parser = this.call("getaccountaddress",
                new Object[] {account});
        return parseAddress(parser);
    }

    /**
     * Moves BTC from one account to another.
     * @param fromAccount the source account.
     * @param toAccount the destination account.
     * @param amount the amount to move
     * @throws ServerErrorException communication error or insufficient funds.
     */
    public final void move(
        final String fromAccount,
        final String toAccount,
        final BTC amount)
        throws ServerErrorException {
        // The API allows accounts to have negative balances. This library
        // prevents it.
        BTC fromAccountBalance = this.getBalance(fromAccount);
        if (amount.longValue() > fromAccountBalance.longValue()) {
            throw new ServerErrorException(String.format(
                    "Account [%s] has insufficient funds.", fromAccount));
        }

        Object[] args = new Object[] {fromAccount, toAccount,
                amount.floatValue()};
        JsonParser parser = this.call("move", args);
        boolean result = parseBoolean(parser);
        if (!result) {
            throw new ServerErrorException("The Move failed.");
        }
    }

    /**
     * @param fromAccount - the name of the account from which to deduct funds
     * @param toAddress - the bitcoin address the amount is sent to
     * @param amount - the amount to transfer
     * @return transaction id
     * @throws ServerErrorException if the call fails
     */
    public final String sendFrom(
        final String fromAccount,
        final String toAddress,
        final BTC amount)
            throws ServerErrorException {
        Object[] args = new Object[] {fromAccount, toAddress,
                amount.floatValue()};
        JsonParser parser = this.call("sendfrom", args);
        return parseTransactionID(parser);
    }

    /**
     * @param parser reference to the parser that contains the id
     * @return the parsed transaction id
     * @throws ServerErrorException when the result cannot be parsed
     */
    private static String parseTransactionID(final JsonParser parser)
            throws ServerErrorException {
        try {
            parser.nextToken();
            while (parser.nextToken() != JsonToken.END_OBJECT) {
                String fieldName = parser.getCurrentName();
                parser.nextToken();
                if (fieldName.equalsIgnoreCase("result")) {
                    return parser.getText();
                }
            }
        } catch (Exception ex) {
            throw new ServerErrorException(ex);
        }

        return null;
    }

    /**
     * @param parser the reference to the parser from which the value is parsed
     * @return the parsed balance
     * @throws ServerErrorException when the amount cannot be parsed
     */
    private static BTC parseBalance(final JsonParser parser)
            throws ServerErrorException {
        try {
            parser.nextToken();
            while (parser.nextToken() != JsonToken.END_OBJECT) {
                String fieldName = parser.getCurrentName();
                parser.nextToken();
                if (fieldName.equalsIgnoreCase("result")) {
                    return new BTC(parser.getFloatValue());
                }
            }
        } catch (Exception ex) {
            throw new ServerErrorException(ex);
        }

        return null;
    }

    /**
     * @param parser reference to the parser that contains the result
     * @return the parsed address
     * @throws ServerErrorException when the address cannot be parsed.
     */
    private static String parseAddress(final JsonParser parser)
            throws ServerErrorException {
        try {
            parser.nextToken();
            while (parser.nextToken() != JsonToken.END_OBJECT) {
                String fieldName = parser.getCurrentName();
                parser.nextToken();
                if (fieldName.equalsIgnoreCase("result")) {
                    return parser.getText();
                }
            }
        } catch (Exception ex) {
            throw new ServerErrorException(ex);
        }

        return null;
    }

    /**
     * @param parser - reference to the parser which has content to parse.
     * @return the parsed value.
     * @throws ServerErrorException - when the value cannot be parsed.
     */
    private static boolean parseBoolean(final JsonParser parser)
            throws ServerErrorException {
        try {
            parser.nextToken();
            while (parser.nextToken() != JsonToken.END_OBJECT) {
                String fieldName = parser.getCurrentName();
                parser.nextToken();
                if (fieldName.equalsIgnoreCase("result")) {
                    return parser.getBooleanValue();
                }
            }
        } catch (Exception ex) {
            throw new ServerErrorException(ex);
        }

        return false;
    }

    /**
     * @param parser reference to the parser.
     * @return the parsed bitcoin info object.
     * @throws ServerErrorException when the structure cannot be parsed.
     */
    private static BitcoinInfo parseBitcoinInfo(final JsonParser parser)
            throws ServerErrorException {
        try {
            BitcoinInfo result = new BitcoinInfo();
            parser.nextToken();
            while (parser.nextToken() != JsonToken.END_OBJECT) {
                String fieldName = parser.getCurrentName();
                parser.nextToken();
                if (fieldName.equalsIgnoreCase("version")) {
                    result.setVersion(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("protocolversion")) {
                    result.setProtocolVersion(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("walletversion")) {
                    result.setWalletVersion(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("balance")) {
                    result.setBalance(new BTC(parser.getFloatValue()));
                } else if (fieldName.equalsIgnoreCase("blocks")) {
                    result.setBlocks(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("connections")) {
                    result.setConnections(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("proxy")) {
                    result.setProxy(parser.getText());
                } else if (fieldName.equalsIgnoreCase("difficulty")) {
                    result.setDifficulty(parser.getDoubleValue());
                } else if (fieldName.equalsIgnoreCase("testnet")) {
                    result.setTestnet(parser.getBooleanValue());
                } else if (fieldName.equalsIgnoreCase("keypoololdest")) {
                    result.setKeyPoolOldest(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("keypoolsize")) {
                    result.setKeyPoolSize(parser.getIntValue());
                } else if (fieldName.equalsIgnoreCase("paytxfee")) {
                    result.setPayTxFee(new BTC(parser.getFloatValue()));
                } else if (fieldName.equalsIgnoreCase("errors")) {
                    result.setErrors(parser.getText());
                }
            }

            return result;
        } catch (Exception ex) {
            throw new ServerErrorException(ex);
        }
    }

    /**
     * @param parser
     *            the parser object
     * @param inner
     *            the inner error
     * @return a wrapped exception
     * @throws IOException
     *             when the error could not be read
     */
    private static ServerErrorException parseError(
        final JsonParser parser,
        final Exception inner) throws IOException {
        String message = "";
        int code = 0;

        parser.nextToken();
        while (parser.nextToken() != JsonToken.END_OBJECT) {
            String fieldName = parser.getCurrentName();
            parser.nextToken();
            if (fieldName.equalsIgnoreCase("error")) {
                while (parser.nextToken() != JsonToken.END_OBJECT) {
                    String innerFieldName = parser.getCurrentName();
                    parser.nextToken();
                    if (innerFieldName.equalsIgnoreCase("code")) {
                        code = parser.getIntValue();
                    } else if (innerFieldName.equalsIgnoreCase("message")) {
                        message = parser.getText();
                    }
                }
            }
        }

        ServerErrorException error = new ServerErrorException(message, inner);
        error.setCode(code);
        return error;
    }

    /**
     * @param method
     *            - the method to call.
     * @param args
     *            - the args to the method.
     * @return - a parser object for the return message.
     * @throws ServerErrorException
     *             - when communication fails
     */
    private JsonParser call(final String method, final Object[] args)
            throws ServerErrorException {
        Authenticator.setDefault(this.auth);

        try {
            HttpURLConnection connection = (HttpURLConnection) this.url
                    .openConnection();
            connection.setRequestMethod("POST");
            connection
                    .addRequestProperty("ContentType", "application/json-rpc");
            connection.setDoOutput(true);
            OutputStream requestStream = connection.getOutputStream();
            JsonGenerator generator = this.factory
                    .createJsonGenerator(requestStream);
            generator.writeStartObject();
            generator.writeStringField("jsonrpc", "1.0");
            generator.writeStringField("id", "1");
            generator.writeStringField("method", method);
            generator.writeArrayFieldStart("params");
            for (int i = 0; i < args.length; i++) {
                generator.writeObject(args[i]);
            }

            generator.writeEndArray();
            generator.writeEndObject();
            generator.close();

            try {
                return this.factory.createJsonParser(connection
                        .getInputStream());
            } catch (IOException ex) {
                if (!connection.getContentType().contains("json")) {
                    throw new ServerErrorException(ex);
                }

                JsonParser errorParser = this.factory
                        .createJsonParser(connection.getErrorStream());
                throw parseError(errorParser, ex);
            }
        } catch (IOException exex) {
            throw new ServerErrorException(exex);
        }
    }
}
