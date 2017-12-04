function createHeadModel() {
    var posData = [
        [1.000000, 1.000000, -1.000000],
        [1.000000, -1.000000, -1.000000],
        [-1.000000, -1.000000, -1.000000],
        [-1.000000, 1.000000, -1.000000],
        [1.000000, 1, 1.000000],
        [1, -1.000001, 1.000000],
        [-1.000000, -1.000000, 1.000000],
        [-1.000000, 1.000000, 1.000000],
        [-1.125, -1.125, -1.125],
        [-1.125, -1.125, 1.125],
        [-1.125, 1.125, -1.125],
        [-1.125, 1.125, 1.125],
        [1.125, -1.125, -1.125],
        [1.125, -1.125, 1.125],
        [1.125, 1.125, -1.125],
        [1.125, 1.125, 1.125]
    ];

    var uvData = [
        [0.2500, 0.7500],
        [0.3750, 1.0000],
        [0.3750, 0.7500],
        [0.1250, 1.0000],
        [0.2500, 0.7500],
        [0.2500, 1.0000],
        [0.1250, 0.7500],
        [0.0000, 0.5000],
        [0.1250, 0.5000],
        [0.5000, 0.7500],
        [0.3750, 0.5000],
        [0.5000, 0.5000],
        [0.3750, 0.5000],
        [0.2500, 0.7500],
        [0.3750, 0.7500],
        [0.1250, 0.7500],
        [0.2500, 0.5000],
        [0.2500, 0.7500],
        [0.8750, 0.7500],
        [0.7500, 0.5000],
        [0.8750, 0.5000],
        [0.7500, 0.7500],
        [0.6250, 0.5000],
        [0.7500, 0.5000],
        [0.6250, 0.7500],
        [0.5000, 0.5000],
        [0.6250, 0.5000],
        [1.0000, 0.7500],
        [0.8750, 0.5000],
        [1.0000, 0.5000],
        [0.7500, 0.7500],
        [0.6250, 1.0000],
        [0.6250, 0.7500],
        [0.2500, 0.7500],
        [0.2500, 1.0000],
        [0.3750, 1.0000],
        [0.1250, 1.0000],
        [0.1250, 0.7500],
        [0.2500, 0.7500],
        [0.1250, 0.7500],
        [0.0000, 0.7500],
        [0.0000, 0.5000],
        [0.5000, 0.7500],
        [0.3750, 0.7500],
        [0.3750, 0.5000],
        [0.3750, 0.5000],
        [0.2500, 0.5000],
        [0.2500, 0.7500],
        [0.1250, 0.7500],
        [0.1250, 0.5000],
        [0.2500, 0.5000],
        [0.8750, 0.7500],
        [0.7500, 0.7500],
        [0.7500, 0.5000],
        [0.7500, 0.7500],
        [0.6250, 0.7500],
        [0.6250, 0.5000],
        [0.6250, 0.7500],
        [0.5000, 0.7500],
        [0.5000, 0.5000],
        [1.0000, 0.7500],
        [0.8750, 0.7500],
        [0.8750, 0.5000],
        [0.7500, 0.7500],
        [0.7500, 1.0000],
        [0.6250, 1.0000]
    ];

    var texCoordIndex = [
        [0, 1, 2],
        [3, 4, 5],
        [6, 7, 8],
        [9, 10, 11],
        [12, 13, 14],
        [15, 16, 17],
        [18, 19, 20],
        [21, 22, 23],
        [24, 25, 26],
        [27, 28, 29],
        [30, 31, 32],
        [33, 34, 35],
        [36, 37, 38],
        [39, 40, 41],
        [42, 43, 44],
        [45, 46, 47],
        [48, 49, 50],
        [51, 52, 53],
        [54, 55, 56],
        [57, 58, 59],
        [60, 61, 62],
        [63, 64, 65]
    ];

    var coordIndex = [
        [0, 2, 3],
        [7, 5, 4],
        [4, 1, 0],
        [5, 2, 1],
        [2, 7, 6],
        [7, 0, 4],
        [9, 10, 8],
        [11, 14, 10],
        [15, 12, 14],
        [13, 8, 12],
        [11, 13, 15],
        [0, 1, 2],
        [7, 6, 5],
        [4, 5, 1],
        [5, 6, 2],
        [2, 3, 7],
        [7, 3, 0],
        [9, 11, 10],
        [11, 15, 14],
        [15, 13, 12],
        [13, 9, 8],
        [11, 9, 13]
    ];

    var geometry = new THREE.Geometry();
    for (var i = 0; i < posData.length; i++) {
        var v = posData[i];
        geometry.vertices.push(new THREE.Vector3(
            v[0],
            v[1],
            v[2]));
    }

    for (var i = 0; i < coordIndex.length; i++) {
        var faceIndex = coordIndex[i];
        var uvIndex = texCoordIndex[i];

        var s0 = uvData[uvIndex[0]];
        var s1 = uvData[uvIndex[1]];
        var s2 = uvData[uvIndex[2]];

        var t0 = new THREE.Vector2(
            s0[0],
            s0[1]);

        var t1 = new THREE.Vector2(
            s1[0],
            s1[1]);

        var t2 = new THREE.Vector2(
            s2[0],
            s2[1]);

        geometry.faces.push(
            new THREE.Face3(faceIndex[0], faceIndex[1], faceIndex[2]));
        geometry.faceVertexUvs[0].push([
            t0,
            t1,
            t2,
        ]);
    }

    geometry.computeFaceNormals();
    return geometry;
}

function createHeadRenderer(playerId) {
    var scene = new THREE.Scene();
    scene.background = new THREE.Color(0xffffff);
    var aspect = 1.0;
    var camera = new THREE.PerspectiveCamera(50, aspect, 0.1, 1000);
    var renderer = new THREE.WebGLRenderer();
    renderer.setSize(400, 400);

    var light = new THREE.DirectionalLight(0xffffff, 0.8);
    light.position.set(-8, 8, 8);
    scene.add(light);
    var light2 = new THREE.AmbientLight(0x444444);
    scene.add(light2);

    var geometry = createHeadModel();
    var loader = new THREE.ImageLoader();
    loader.load(
        '/api/v1/player/' + playerId + '/skin',
        function (image) {
            var canvas = document.createElement('canvas');
            canvas.width = 64;
            canvas.height = 32;
            var context = canvas.getContext('2d');
            context.drawImage(image, 0, 0);

            var texture = new THREE.CanvasTexture(canvas);

            texture.magFilter = THREE.NearestFilter;
            texture.mapFilter = THREE.NearestFilter;
            texture.needsUpdate = true;
            var material = new THREE.MeshBasicMaterial({
                color: 0xffffff,
                map: texture
            });

            material.alphaTest = 0.5;
            //material.lights = true;
            material.side = THREE.DoubleSide;

            var cube = new THREE.Mesh(geometry, material);

            scene.add(cube);
            cube.rotation.x = Math.PI / 2.0;
            cube.rotation.y = Math.PI;
            camera.rotateX(-Math.PI / 8.0);
            camera.translateZ(5.0);

            var render = function () {
                requestAnimationFrame(render);
                cube.rotateZ(0.01);
                renderer.render(scene, camera);
            };

            render();
        });

    return renderer;
}