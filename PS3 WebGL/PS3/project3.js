// [TO-DO] Complete the implementation of the following class and the vertex shader below.

class CurveDrawer {
	constructor()
	{
		// Compile the shader program
		this.prog   = InitShaderProgram( curvesVS, curvesFS );
		// [TO-DO] Other initializations should be done here.
		// [TO-DO] This is a good place to get the locations of attributes and uniform variables.

		// Get the ids of the uniform variables in the shaders
		this.mvp = gl.getUniformLocation(this.prog, 'mvp');
		this.p0 = gl.getUniformLocation(this.prog, 'p0');
		this.p1 = gl.getUniformLocation(this.prog, 'p1');
		this.p2 = gl.getUniformLocation(this.prog, 'p2');
		this.p3 = gl.getUniformLocation(this.prog, 'p3');
		this.tVals = gl.getAttribLocation(this.prog, 't');
		// Initialize the attribute buffer
		this.steps = 100;
		var tv = [];
		for ( var i=0; i<this.steps; ++i ) {
			tv.push( i / (this.steps-1) );
		}
		// [TO-DO] This is where you can create and set the contents of the vertex buffer object
		// for the vertex attribute we need.

		// Create the vertex buffer object
		this.buffer = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, this.buffer);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(tv), gl.STATIC_DRAW);
	}
	setViewport( width, height )
	{


		var trans = [2 / width, 0, 0, 0, 0, -2 / height, 0, 0, 0, 0, 1, 0, -1, 1, 0, 1];
		gl.useProgram(this.prog); // Bind the program
		gl.uniformMatrix4fv(this.mvp, false, trans);
		// [TO-DO] This is where we should set the transformation matrix.
		// [TO-DO] Do not forget to bind the program before you set a uniform variable value.
			
	}
	updatePoints( pt )
	{
		gl.useProgram(this.prog);	// Bind the program

		var p = [];
		for (var i = 0; i < 4; ++i) {
			var x = pt[i].getAttribute("cx");
			var y = pt[i].getAttribute("cy");
			p.push([x, y]);
		}
		console.log("- ", p[0][0], "-", p[0][1]);
		console.log("- ", p[1][0], "-", p[1][1]);
		console.log("-", p[2][0], "-", p[2][1]);
		console.log("-", p[3][0], "-", p[3][1]);

		gl.uniform2fv(this.p0, p[0]);
		gl.uniform2fv(this.p1, p[1]);
		gl.uniform2fv(this.p2, p[2]);
		gl.uniform2fv(this.p3, p[3]); 

		
		// [TO-DO] The control points have changed, we must update corresponding uniform variables.
		// [TO-DO] Do not forget to bind the program before you set a uniform variable value.
		// [TO-DO] We can access the x and y coordinates of the i^th control points using
		
	}
	draw()
	{

		gl.useProgram(this.prog);
		gl.bindBuffer(gl.ARRAY_BUFFER, this.buffer);
		gl.vertexAttribPointer(this.tVals, 1, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(this.tVals);
		gl.drawArrays(gl.LINE_STRIP, 0, this.steps);
		// [TO-DO] This is where we give the command to draw the curve.
		// [TO-DO] Do not forget to bind the program and set the vertex attribute.
	}
}

// Vertex Shader
var curvesVS = `
	attribute float t;
	uniform mat4 mvp;
	uniform vec2 p0;
	uniform vec2 p1;
	uniform vec2 p2;
	uniform vec2 p3;
	void main()
	{
		// [TO-DO] Replace the following with the proper vertex shader code
		float invT = 1.0 - t;
		vec2 pos = (pow(invT,3.0)* p0) + (3.0 * pow(invT,2.0) * t * p1) + (3.0 * invT * t*t * p2) + (t*t*t * p3);
		gl_Position = mvp * vec4(pos, 0, 1);
	}
`;

// Fragment Shader
var curvesFS = `
	precision mediump float;
	void main()
	{
		gl_FragColor = vec4(1,0,0,1);
	}
`;