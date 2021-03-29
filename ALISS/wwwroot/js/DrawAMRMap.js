   window.MoveCanvas = (divCanvas, divImage, x, y, divRegion,idx) => {
        var imgBigMap = document.getElementById(divImage);
        var cnvs = document.getElementById(divCanvas);
        var imgRegion = document.getElementById(divRegion);

        var scale = Math.min(cnvs.width / imgRegion.width, cnvs.height / imgRegion.height);

        cnvs.style.position = "absolute";
        cnvs.style.left = (imgBigMap.offsetLeft + x) + "px";
        cnvs.style.top = (imgBigMap.offsetTop + y) + "px";

        var ctx = cnvs.getContext("2d");
        ctx.clearRect(0, 0, ctx.width, ctx.height);
        ctx.stroke();
        ctx.drawImage(imgRegion, 0, 0, imgRegion.width * scale, imgRegion.height * scale);
        ctx.font = "18px";
        ctx.fillText(idx, imgRegion.width / 2, imgRegion.height / 2);
    }
	
	window.DrawValue = (divCanvas, divImage, x, y, txtValue) =>  {
        var cnvs = document.getElementById(divCanvas);
        var img = document.getElementById(divImage);

        cnvs.style.position = "absolute";
        cnvs.style.left = (img.offsetLeft + x) + "px";
        cnvs.style.top = (img.offsetTop + y) + "px";
         
        var ctx = cnvs.getContext("2d");
        ctx.beginPath();
        //ctx.clearRect(0, 0, cnvs.width, cnvs.height);
       
        ctx.font = "15px Arial";        
        ctx.fillText(txtValue,0 ,cnvs.height/2);
    }
	
    window.SetPositionCanvasInitial = (divImage,divCVS) => {       
        var img = document.getElementById(divImage);
        var cnvs01 = document.getElementById(divCVS[0]);
        var cnvs02 = document.getElementById(divCVS[1]);
        var cnvs03 = document.getElementById(divCVS[2]);
        var cnvs04 = document.getElementById(divCVS[3]);
        var cnvs05 = document.getElementById(divCVS[4]);
        var cnvs06 = document.getElementById(divCVS[5]);
        var cnvs07 = document.getElementById(divCVS[6]);
        var cnvs08 = document.getElementById(divCVS[7]);
        var cnvs09 = document.getElementById(divCVS[8]);
        var cnvs10 = document.getElementById(divCVS[9]);
        var cnvs11 = document.getElementById(divCVS[10]);
        var cnvs12 = document.getElementById(divCVS[11]);
        var cnvs13 = document.getElementById(divCVS[12]);

        cnvs01.style.position = "absolute";
        cnvs02.style.position = "absolute";
        cnvs03.style.position = "absolute";
        cnvs04.style.position = "absolute";
        cnvs05.style.position = "absolute";
        cnvs06.style.position = "absolute";
        cnvs07.style.position = "absolute";
        cnvs08.style.position = "absolute";
        cnvs09.style.position = "absolute";
        cnvs10.style.position = "absolute";
        cnvs11.style.position = "absolute";
        cnvs12.style.position = "absolute";
        cnvs13.style.position = "absolute";
          
        var fontvalue = "15px Arial";
        var fontcolor = "#999999";
        var row1_x = 41;
        var row2_x = 165;
        var txtValue = "0.0%";

        cnvs01.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs01.style.top = (img.offsetTop - 11) + "px";  

        cnvs02.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs02.style.top = (img.offsetTop + 32) + "px"; 

        cnvs03.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs03.style.top = (img.offsetTop + 77) + "px";  

        cnvs04.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs04.style.top = (img.offsetTop + 126) + "px";

        cnvs05.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs05.style.top = (img.offsetTop + 175) + "px";

        cnvs06.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs06.style.top = (img.offsetTop + 218) + "px";

        cnvs07.style.left = (img.offsetLeft + row1_x) + "px";         
        cnvs07.style.top = (img.offsetTop + 269) + "px";

        cnvs08.style.left = (img.offsetLeft + row2_x) + "px";         
        cnvs08.style.top = (img.offsetTop - 11) + "px";

        cnvs09.style.left = (img.offsetLeft + row2_x) + "px";         
        cnvs09.style.top = (img.offsetTop + 34) + "px";

        cnvs10.style.left = (img.offsetLeft + row2_x) + "px";         
        cnvs10.style.top = (img.offsetTop + 79) + "px";

        cnvs11.style.left = (img.offsetLeft + row2_x) + "px";         
        cnvs11.style.top = (img.offsetTop + 124) + "px";

        cnvs12.style.left = (img.offsetLeft + row2_x) + "px";         
        cnvs12.style.top = (img.offsetTop + 176) + "px";

        cnvs13.style.left = (img.offsetLeft + row2_x) + "px";         
        cnvs13.style.top = (img.offsetTop + 222) + "px";

        var ctx01 = cnvs01.getContext("2d");
        ctx01.beginPath();
        ctx01.font = fontvalue;   
        ctx01.fillStyle = fontcolor;
        ctx01.fillText(txtValue, 0, cnvs01.height / 2);

        var ctx02 = cnvs02.getContext("2d");
        ctx02.beginPath();
        ctx02.font = fontvalue;     
        ctx02.fillStyle = fontcolor;
        ctx02.fillText(txtValue, 0, cnvs02.height / 2);

        var ctx03 = cnvs03.getContext("2d");
        ctx03.beginPath();
        ctx03.font = fontvalue;        
        ctx03.fillStyle = fontcolor;
        ctx03.fillText(txtValue, 0, cnvs03.height / 2);

        var ctx04 = cnvs04.getContext("2d");
        ctx04.beginPath();
        ctx04.font = fontvalue;      
        ctx04.fillStyle = fontcolor;
        ctx04.fillText(txtValue, 0, cnvs04.height / 2);

        var ctx05 = cnvs05.getContext("2d");
        ctx05.beginPath();
        ctx05.font = fontvalue;        
        ctx05.fillStyle = fontcolor;
        ctx05.fillText(txtValue, 0, cnvs05.height / 2);

        var ctx06 = cnvs06.getContext("2d");
        ctx06.beginPath();
        ctx06.font = fontvalue;        
        ctx06.fillStyle = fontcolor;
        ctx06.fillText(txtValue, 0, cnvs06.height / 2);

        var ctx07 = cnvs07.getContext("2d");
        ctx07.beginPath();
        ctx07.font = fontvalue;  
        ctx07.fillStyle = fontcolor;
        ctx07.fillText(txtValue, 0, cnvs07.height / 2);

        var ctx08 = cnvs08.getContext("2d");
        ctx08.beginPath();
        ctx08.font = fontvalue;      
        ctx08.fillStyle = fontcolor;
        ctx08.fillText(txtValue, 0, cnvs08.height / 2);

        var ctx09 = cnvs09.getContext("2d");
        ctx09.beginPath();
        ctx09.font = fontvalue;  
        ctx09.fillStyle = fontcolor;
        ctx09.fillText(txtValue, 0, cnvs09.height / 2);

        var ctx10 = cnvs10.getContext("2d");
        ctx10.beginPath();
        ctx10.font = fontvalue;  
        ctx10.fillStyle = fontcolor;
        ctx10.fillText(txtValue, 0, cnvs10.height / 2);

        var ctx11 = cnvs11.getContext("2d");
        ctx11.beginPath();
        ctx11.font = fontvalue;        
        ctx11.fillStyle = fontcolor;
        ctx11.fillText(txtValue, 0, cnvs11.height / 2);

        var ctx12 = cnvs12.getContext("2d");
        ctx12.beginPath();
        ctx12.font = fontvalue;       
        ctx12.fillStyle = fontcolor;
        ctx12.fillText(txtValue, 0, cnvs12.height / 2);

        var ctx13 = cnvs13.getContext("2d");
        ctx13.beginPath();
        ctx13.font = fontvalue; 
        ctx13.fillStyle = fontcolor;
        ctx13.fillText(txtValue,0 ,cnvs13.height/2);

    }
	
    window.setPercentValue = (divcvs, txtValue) =>  {
        var cnvs01 = document.getElementById(divcvs[0]);
        var cnvs02 = document.getElementById(divcvs[1]);
        var cnvs03 = document.getElementById(divcvs[2]);
        var cnvs04 = document.getElementById(divcvs[3]);
        var cnvs05 = document.getElementById(divcvs[4]);
        var cnvs06 = document.getElementById(divcvs[5]);
        var cnvs07 = document.getElementById(divcvs[6]);
        var cnvs08 = document.getElementById(divcvs[7]);
        var cnvs09 = document.getElementById(divcvs[8]);
        var cnvs10 = document.getElementById(divcvs[9]);
        var cnvs11 = document.getElementById(divcvs[10]);
        var cnvs12 = document.getElementById(divcvs[11]);
        var cnvs13 = document.getElementById(divcvs[12]);

        var fontcolor = "#333333";
        var ctx01 = cnvs01.getContext("2d");
        var ctx02 = cnvs02.getContext("2d");
        var ctx03 = cnvs03.getContext("2d");
        var ctx04 = cnvs04.getContext("2d");
        var ctx05 = cnvs05.getContext("2d");
        var ctx06 = cnvs06.getContext("2d");
        var ctx07 = cnvs07.getContext("2d");
        var ctx08 = cnvs08.getContext("2d");
        var ctx09 = cnvs09.getContext("2d");
        var ctx10 = cnvs10.getContext("2d");
        var ctx11 = cnvs11.getContext("2d");
        var ctx12 = cnvs12.getContext("2d");
        var ctx13 = cnvs13.getContext("2d");

        ctx01.beginPath();     
        ctx01.clearRect(0, 0, cnvs01.width, cnvs01.height);
        ctx01.fillStyle = fontcolor;
        ctx01.fillText(txtValue[0] + "%", 0, cnvs01.height / 2);

        ctx02.beginPath();     
        ctx02.clearRect(0, 0, cnvs02.width, cnvs01.height);
        ctx02.fillStyle = fontcolor;
        ctx02.fillText(txtValue[1] + "%", 0, cnvs02.height / 2);

        ctx03.beginPath();     
        ctx03.clearRect(0, 0, cnvs03.width, cnvs01.height);
        ctx03.fillStyle = fontcolor;
        ctx03.fillText(txtValue[2] + "%", 0, cnvs03.height / 2);

        ctx04.beginPath();     
        ctx04.clearRect(0, 0, cnvs04.width, cnvs01.height);
        ctx04.fillStyle = fontcolor;
        ctx04.fillText(txtValue[3] + "%", 0, cnvs04.height / 2);

        ctx05.beginPath();     
        ctx05.clearRect(0, 0, cnvs05.width, cnvs01.height);
        ctx05.fillStyle = fontcolor;
        ctx05.fillText(txtValue[4] + "%", 0, cnvs05.height / 2);

        ctx06.beginPath();     
        ctx06.clearRect(0, 0, cnvs06.width, cnvs01.height);
        ctx06.fillStyle = fontcolor;
        ctx06.fillText(txtValue[5] + "%", 0, cnvs06.height / 2);

        ctx07.beginPath();     
        ctx07.clearRect(0, 0, cnvs07.width, cnvs01.height);
        ctx07.fillStyle = fontcolor;
        ctx07.fillText(txtValue[6] + "%", 0, cnvs07.height / 2);

        ctx08.beginPath();     
        ctx08.clearRect(0, 0, cnvs08.width, cnvs01.height);
        ctx08.fillStyle = fontcolor;
        ctx08.fillText(txtValue[7] + "%", 0, cnvs08.height / 2);

        ctx09.beginPath();     
        ctx09.clearRect(0, 0, cnvs09.width, cnvs01.height);
        ctx09.fillStyle = fontcolor;
        ctx09.fillText(txtValue[8] + "%", 0, cnvs09.height / 2);

        ctx10.beginPath();     
        ctx10.clearRect(0, 0, cnvs10.width, cnvs01.height);
        ctx10.fillStyle = fontcolor;
        ctx10.fillText(txtValue[9] + "%", 0, cnvs10.height / 2);

        ctx11.beginPath();     
        ctx11.clearRect(0, 0, cnvs11.width, cnvs01.height);
        ctx11.fillStyle = fontcolor;
        ctx11.fillText(txtValue[10] + "%", 0, cnvs11.height / 2);

        ctx12.beginPath();     
        ctx12.clearRect(0, 0, cnvs12.width, cnvs01.height);
        ctx12.fillStyle = fontcolor;
        ctx12.fillText(txtValue[11] + "%", 0, cnvs12.height / 2);

        ctx13.beginPath();     
        ctx13.clearRect(0, 0, cnvs13.width, cnvs01.height);
        ctx13.fillStyle = fontcolor;
        ctx13.fillText(txtValue[12] + "%", 0, cnvs13.height / 2);

        }