// This function takes the projection matrix, the translation, and two rotation angles (in radians) as input arguments.
// The two rotations are applied around x and y axes.
// It returns the combined 4x4 transformation matrix as an array in column-major order.
// The given projection matrix is also a 4x4 matrix stored as an array in column-major order.
// You can use the MatrixMult function defined in project4.html to multiply two 4x4 matrices in the same format.
function GetModelViewProjection(projectionMatrix, translationX, translationY, translationZ, rotationX, rotationY) {
	var matrixX = [
		1, 0, 0, 0,
		0, Math.cos(rotationX), Math.sin(rotationX), 0,
		0, -Math.sin(rotationX), Math.cos(rotationX), 0,
		0, 0, 0, 1
	];
	var matrixY = [
		Math.cos(rotationY), 0, -Math.sin(rotationY), 0,
		0, 1, 0, 0,
		Math.sin(rotationY), 0, Math.cos(rotationY), 0,
		0, 0, 0, 1
	];
	var trans = [
		1, 0, 0, 0,
		0, 1, 0, 0,
		0, 0, 1, 0,
		translationX, translationY, translationZ, 1
	];

	var transMatrix = MatrixMult(trans, MatrixMult(matrixX, matrixY));
	var mvp = MatrixMult(projectionMatrix, transMatrix);
	return mvp;
}


// [TO-DO] Complete the implementation of the following class.

class MeshDrawer {
	// The constructor is a good place for taking care of the necessary initializations.
	constructor() {
		// Start the program
		this.prog = InitShaderProgram(meshVS, meshFS);

		// Get the ids of the uniform variables in the shaders
		this.mvp = gl.getUniformLocation(this.prog, 'mvp');
		this.sampler = gl.getUniformLocation(this.prog, 'tex');
		this.swap = gl.getUniformLocation(this.prog, 'swap');
		this.show = gl.getUniformLocation(this.prog, 'show');

		// Get the ids of the vertex attributes in the shaders
		this.pos = gl.getAttribLocation(this.prog, 'pos');
		this.txc = gl.getAttribLocation(this.prog, 'txc');

		// Create buffers
		this.posBuffer = gl.createBuffer();
		this.texBuffer = gl.createBuffer();
	}

	// This method is called every time the user opens an OBJ file.
	// The arguments of this function is an array of 3D vertex positions
	// and an array of 2D texture coordinates.
	// Every item in these arrays is a floating point value, representing one
	// coordinate of the vertex position or texture coordinate.
	// Every three consecutive elements in the vertPos array forms one vertex
	// position and every three consecutive vertex positions form a triangle.
	// Similarly, every two consecutive elements in the texCoords array
	// form the texture coordinate of a vertex.
	// Note that this method can be called multiple times.
	setMesh(vertPos, texCoords) {
		// update the object
		this.numTriangles = vertPos.length / 3;

		//get vertPos
		gl.bindBuffer(gl.ARRAY_BUFFER, this.posBuffer);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(vertPos), gl.STATIC_DRAW);

		//get vertCoords
		gl.bindBuffer(gl.ARRAY_BUFFER, this.texBuffer);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(texCoords), gl.STATIC_DRAW);
	}

	// This method is called when the user changes the state of the
	// "Swap Y-Z Axes" checkbox. 
	// The argument is a boolean that indicates if the checkbox is checked.
	swapYZ(swap) {
		gl.useProgram(this.prog);
		//flip
		gl.uniform1i(this.swap, swap);
	}

	// This method is called to draw the triangular mesh.
	// The argument is the transformation matrix, the same matrix returned
	// by the GetModelViewProjection function above.
	draw(trans) {

		gl.useProgram(this.prog);

		//link the matrix
		gl.uniformMatrix4fv(this.mvp, false, trans);

		//get vert pos buffer
		gl.bindBuffer(gl.ARRAY_BUFFER, this.posBuffer);
		gl.vertexAttribPointer(this.pos, 3, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(this.pos);

		//get texture buffer
		gl.bindBuffer(gl.ARRAY_BUFFER, this.texBuffer);
		gl.vertexAttribPointer(this.txc, 2, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(this.txc);

		//draw line between them
		gl.drawArrays(gl.TRIANGLES, 0, this.numTriangles);
	}

	// This method is called to set the texture of the mesh.
	// The argument is an HTML IMG element containing the texture data.
	setTexture(img) {
		//Bind the texture
		const t = gl.createTexture();
		gl.bindTexture(gl.TEXTURE_2D, t)

		//set value here
		gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGB, gl.RGB, gl.UNSIGNED_BYTE, img);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR_MIPMAP_LINEAR);

		//generate and start
		gl.generateMipmap(gl.TEXTURE_2D);
		gl.activeTexture(gl.TEXTURE0);
		gl.useProgram(this.prog);
		gl.uniform1i(this.show, 1);
	}

	// This method is called when the user changes the state of the
	// "Show Texture" checkbox. 
	// The argument is a boolean that indicates if the checkbox is checked.
	showTexture(show) {
		gl.useProgram(this.prog);
		gl.uniform1i(this.show, show);
	}
}

// Vertex Shader
var meshVS = `
	attribute vec3 pos;
	attribute vec2 txc;
	varying vec2 texC;
	uniform mat4 mvp;
	uniform int swap;
	void main()
	{
		if (swap == 1) {
			gl_Position = mvp * vec4(pos[0], pos[2], pos[1], 1);
	
		}
		else {
			gl_Position = mvp * vec4(pos, 1);
		}
		texC = txc;
	}
`;

// Fragment Shader
var meshFS = `
	precision mediump float;
	uniform sampler2D tex;
	varying vec2 texC;
	uniform int show;
	void main()
	{
		if (show == 1) {
			gl_FragColor = texture2D(tex, texC);
		}
		else {
			gl_FragColor = vec4(1, gl_FragCoord.z*gl_FragCoord.z, 0, 1);
		}
	}
`;