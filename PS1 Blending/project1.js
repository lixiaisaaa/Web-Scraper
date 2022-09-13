// bgImg is the background image to be modified.
// fgImg is the foreground image.
// fgOpac is the opacity of the foreground image.
// fgPos is the position of the foreground image in pixels. It can be negative and (0,0) means the top-left pixels of the foreground and background are aligned.
function composite(bgImg, fgImg, fgOpac, fgPos) {

	//Initial the variable
	let bgX, bgY, fgX, fgY, ab, af, alpha, fgArray, bgArray;

	if (fgPos.x < 0) {
		bgX = 0;
		fgX = Math.abs(fgPos.x);
	} else {
		bgX = fgPos.x;
		fgX = 0;
	}

	if (fgPos.y < 0) {
		bgY = 0;
		fgY = Math.abs(fgPos.y);
	} else{
		bgY = fgPos.y;
		fgY = 0;
	}

	const startYBg = bgY
	const startYFg = fgY;

	// alpha blend for each pixel
	while (fgX < fgImg.width && bgX < bgImg.width) {
		while (fgY < fgImg.height && bgY < bgImg.height) {
			
			fgArray = getColorIndices(fgX, fgY, fgImg.width);
			bgArray = getColorIndices(bgX, bgY, bgImg.width);

			af = fgImg.data[fgArray[3]] / 255;
			ab = bgImg.data[bgArray[3]] / 255;
			
			alpha = (af * fgOpac) + ((1 - (af * fgOpac)) * ab);

			bgImg.data[bgArray[0]] = ((af * fgOpac) * (fgImg.data[fgArray[0]]) + ((1 - (af * fgOpac)) * (ab) * bgImg.data[bgArray[0]])) / alpha;
			bgImg.data[bgArray[1]] = ((af * fgOpac) * (fgImg.data[fgArray[1]]) + ((1 - (af * fgOpac)) * (ab) * bgImg.data[bgArray[1]])) / alpha;
			bgImg.data[bgArray[2]] = ((af * fgOpac) * (fgImg.data[fgArray[2]]) + ((1 - (af * fgOpac)) * (ab) * bgImg.data[bgArray[2]])) / alpha;
			bgImg.data[bgArray[3]] = alpha * 255;

			
			bgY++;
			fgY++;
		}
		//reset the starting Y
		bgY = startYBg;
		fgY = startYFg;

		bgX++;
		fgX++;
	}
}

//Helper function to get the color index
function getColorIndices(x,y,width) {
	var startIndex = y * (width * 4) + x * 4;
	return [startIndex, startIndex + 1, startIndex + 2, startIndex + 3];
}