// Returns a 3x3 transformation matrix as an array of 9 values in column-major order.
// The transformation first applies scale, then rotation, and finally translation.
// The given rotation value is in degrees.
function GetTransform( positionX, positionY, rotation, scale )
{
	//initial the value 
	let rad, cost, sint, scaleMatrix, transMatrix, rotationMatrix;
	rad = rotation * (Math.PI / 180) ;
	cost = Math.cos(rad);
	sint = Math.sin(rad);

	//get 3 matrix that we need
	transMatrix = [1, 0, 0, 0, 1, 0, positionX, positionY, 1];
	
    scaleMatrix = [scale, 0, 0, 0, scale, 0, 0, 0, 1];

	rotationMatrix = [cost, sint, 0, -   sint, cost, 0, 0, 0, 1];

	return ApplyTransform(ApplyTransform(scaleMatrix, rotationMatrix), transMatrix);
}

// Returns a 3x3 transformation matrix as an array of 9 values in column-major order.
// The arguments are transformation matrices in the same format.
// The returned transformation first applies trans1 and then trans2.
function ApplyTransform( trans1, trans2 )
{
	//perform 3x3 matrix multiplication
	let matrix = [0, 0, 0, 0, 0, 0, 0, 0, 0], i = 0;
	for (let r = 0; r < 3; r++) {
		for (let c = 0; c < 3; c++) {
			matrix[i] = trans1[r * 3] * trans2[c] + trans1[r * 3 + 1] * trans2[c + 3] + trans1[r * 3 + 2] * trans2[c + 6];
			i++;
		}
	}

	return matrix;
}
