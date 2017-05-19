//Menu desplegable funciones:
function openNav() {
    document.getElementById("mySidenav").style.width = "10%";
    document.getElementById("main").style.marginLeft = "10%";
    document.getElementById("cuadro2").style.marginLeft = "10%";
    document.getElementById("mySidenav").style.display = "block";
    document.getElementById("openNav").style.display = 'none';

}


function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";
    document.getElementById("cuadro2").style.marginLeft = "0";
}
function handleDragStart(e) {
    this.style.opacity = '0.4';
}

var cols = document.querySelectorAll('#foto .column');
[].forEach.call(cols, function (col) {
    col.addEventListener('dragstart', handleDragStart, false);
});
//drag and drop funciones:
contador = 0; // Variable global para tener poder poner un id unico a cada elemento cuando se clona.
function start(e) {
    e.dataTransfer.effecAllowed = 'move'; // Define el efecto como mover (Es el por defecto)
    e.dataTransfer.setData("Data", e.target.id); // Coje el elemento que se va a mover
    e.dataTransfer.setDragImage(e.target); // Define la imagen que se vera al ser arrastrado el elemento y por donde se coje el elemento que se va a mover (el raton aparece en la esquina sup_izq con 0,0)
    e.target.style.opacity = '0.4';
}

function end(e) {
    e.target.style.opacity = ''; // Pone la opacidad del elemento a 1           
    e.dataTransfer.clearData("Data");
}

function enter(e) {
    e.target.style.border = '3px dotted #555';
}

function leave(e) {
    e.target.style.border = '';
}

function over(e) {
    var elemArrastrable = e.dataTransfer.getData("Data"); // Elemento arrastrado
    var id = e.target.id; // Elemento sobre el que se arrastra

    // return false para que se pueda soltar
    if (id == 'cuadro1') {
        return false; // Cualquier elemento se puede soltar sobre el div destino 1
    }

    if ((id == 'cuadro2') && (elemArrastrable != 'arrastrable3')) {
        return false; // En el cuadro2 se puede soltar cualquier elemento menos el elemento con id=arrastrable3
    }




}


/**
* 
* Mueve el elemento
*
**/
function drop(e) {

    var elementoArrastrado = e.dataTransfer.getData("Data"); // Elemento arrastrado
    e.target.appendChild(document.getElementById(elementoArrastrado));
    e.target.style.border = '';  // Quita el borde
    tamContX = $('#' + e.target.id).width();
    tamContY = $('#' + e.target.id).height();

    tamElemX = $('#' + elementoArrastrado).width();
    tamElemY = $('#' + elementoArrastrado).height();

    posXCont = $('#' + e.target.id).position().left;
    posYCont = $('#' + e.target.id).position().top;


    // Si parte del elemento que se quiere mover se queda fuera se cambia las coordenadas para que no sea asi
    if (posXCont + tamContX <= x + tamElemX) {
        x = posXCont + tamContX - tamElemX;
    }

    if (posYCont + tamContY <= y + tamElemY) {
        y = posYCont + tamContY - tamElemY;
    }

    document.getElementById(elementoArrastrado).style.position = "absolute";
    document.getElementById(elementoArrastrado).style.left = x + "px";
    document.getElementById(elementoArrastrado).style.top = y + "px";
}

/**
* 
* Elimina el elemento que se mueve
*
**/
function eliminar(e) {
    var elementoArrastrado = document.getElementById(e.dataTransfer.getData("Data")); // Elemento arrastrado
    elementoArrastrado.parentNode.removeChild(elementoArrastrado); // Elimina el elemento
    e.target.style.border = '';   // Quita el borde
}

/**
* 
* Clona el elemento que se mueve
*
**/
function clonar(e) {
    var elementoArrastrado = document.getElementById(e.dataTransfer.getData("Data")); // Elemento arrastrado

    elementoArrastrado.style.opacity = ''; // Dejamos la opacidad a su estado anterior para copiar el elemento igual que era antes

    var elementoClonado = elementoArrastrado.cloneNode(true); // Se clona el elemento
    elementoClonado.id = "ElemClonado" + contador; // Se cambia el id porque tiene que ser unico
    contador += 1;
    elementoClonado.style.position = "static";  // Se posiciona de forma "normal" (Sino habria que cambiar las coordenadas de la posición)  
    elementoClonado.style.marginRight = 50 + "px";
    e.target.appendChild(elementoClonado); // Se añade el elemento clonado
    e.target.style.border = '';			// Quita el borde del "cuadro clonador"
}

