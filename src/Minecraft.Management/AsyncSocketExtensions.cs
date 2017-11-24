//-----------------------------------------------------------------------
// <copyright file="AsyncSocketExtensions.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for the socket class to use the async pattern.
    /// </summary>
    internal static class AsyncSocketExtensions
    {
        /// <summary>
        /// Connects the socket asynchronously.
        /// </summary>
        /// <param name="sock">The socket.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns>An awaitable task.</returns>
        public static Task ConnectAsync(
            this Socket sock, 
            string host, 
            int port)
        {
            return Task.Factory.FromAsync(
                sock.BeginConnect,
                sock.EndConnect,
                host,
                port,
                null);
        }

        /// <summary>
        /// Sends asynchronously.
        /// </summary>
        /// <param name="sock">The socket.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>An awaitable task for the number of bytes sent.</returns>
        public static Task<int> SendAsync(
            this Socket sock,
            byte[] buffer)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            AsyncCallback asyncCallback = (asyncResult) =>
            {
                try
                {
                    tcs.TrySetResult(sock.EndSend(asyncResult));
                }
                catch (OperationCanceledException)
                {
                    tcs.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            sock.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, asyncCallback, null);
            return tcs.Task;
        }

        /// <summary>
        /// Receives asynchronously.
        /// </summary>
        /// <param name="sock">The socket.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>An awaitable task for the number of bytes received.</returns>
        public static Task<int> ReceiveAsync(
            this Socket sock,
            byte[] buffer)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            AsyncCallback asyncCallback = (asyncResult) =>
            {
                try
                {
                    tcs.TrySetResult(sock.EndReceive(asyncResult));
                }
                catch (OperationCanceledException)
                {
                    tcs.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            sock.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, asyncCallback, null);
            return tcs.Task;
        }
    }
}
