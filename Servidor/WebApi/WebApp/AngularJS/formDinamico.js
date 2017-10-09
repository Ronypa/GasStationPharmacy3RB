/*
bloque de funciones para realizar las validaciones e inputs
dinamicos para los padecimientos y su fecha
*/
var inputsAdicionales = 1; //cantidad de inputs adecionales agregados
var nombreNuevo = 1; //variable usada para darle un nuevo nombre a cada input creado
function dinamicoPade() { //funcion que crea los inputs
    inputsAdicionales++;
    nombreNuevo++;
    var objTo = document.getElementById('dinamico'); //parte del html formDinamicos donde va el imput
    var divtest = document.createElement("div"); //se crea un bloque div
    divtest.setAttribute("class", "form-group removeclassPade" + nombreNuevo); //se le da nombre
    var rdiv = 'removeclassPade' + nombreNuevo; //nombre
    //la siguiente linea crea el bloque html de los inputs
    divtest.innerHTML = '<div class="col-md-5"><form><input type="text" class="form-control" placeholder="Padecimiento" name="a" ng-model="registroPadecimimeto" oninput="largoAdecuadoPade()" required/></form></div><div class="col-md-5"><form><input type="text" onfocus="(this.type=\'date\')" class="form-control" placeholder="Fecha de pronóstico" ng-model="registroPadecimimetoFecha" oninput="largoAdecuadoPade()" required/></form></div><div class="col-md-1"><button class="btn btn-danger btn-sm" type="button" onclick="remove_pades(' + nombreNuevo + ');"> <span class="glyphicon glyphicon-minus" aria-hidden="true"></span></button></div>';
    objTo.appendChild(divtest);//se adiere el nuevo div
    document.getElementById('botonMasPade').disabled = true; //cada vez que se agrega un input se deshabilita el boton de agregar mas
}
function remove_pades(rid) {//funcion a la que el boton eliminar llama para eliminar a su respectivo input, el parametro es el 'nombreNuevo' correspondiente
    $('.removeclassPade' + rid).remove(); //se eliminan los inputs
    inputsAdicionales--;
    largoAdecuadoPade();
}

function largoAdecuadoPade() { //funcion para determinar si los inputs tienen la informacion 
    var input = document.getElementById('dinamico').firstElementChild;//lista de elementos que tiene el bloque formulariosPade
    if (document.getElementById('padecimiento').value.length !== 0 && document.getElementById('fechaPade').value.length !== 0) {//si los inputs que estan por defecto poseen informacion
        if (inputsAdicionales > 1) {
            while (input !== null) {//iteracion para revisar todos los inputs de los bloques deformularios agregados
                var input2 = input.firstElementChild;
                if (input2.firstElementChild.firstElementChild.value.length !== 0 && input2.nextElementSibling.firstElementChild.firstElementChild.value.length !== 0) {
                    if (input2.nextElementSibling.firstElementChild.firstElementChild.nextElementSibling !== null) {
                        console.log(input2.nextElementSibling.firstElementChild.firstElementChild.value);
                    }
                    document.getElementById('botonMasPade').disabled = false; //si todos estan bien, boton encendido
                }
                else
                    document.getElementById('botonMasPade').disabled = true;//boton apagado
                input = input.nextElementSibling;//siguiente bloque de formularios
            }
        }
        else {
            document.getElementById('botonMasPade').disabled = false;
        }
    }
    else
        document.getElementById('botonMasPade').disabled = true;
}
/*---------------------Fin del bloque de funciones para padecimeintos-----------------------------------*/

/* bloque de funciones para realizar las validaciones e 
inputs dinamicos para los inputs de los telefonos*/
var cantidadTels = 1; //cantidad de nuevos inputs
var nombreTels = 1; //variable para renombrar a los nuevos
function dinamicoTel() {//genera los nuevos inputs para telefonos
    cantidadTels++;
    nombreTels++;
    var objTo = document.getElementById('tels');
    var divtest = document.createElement("div");
    divtest.setAttribute("class", "form-group removeclassTel" + nombreTels);
    var rdiv = 'removeclassTel' + nombreTels;
    divtest.innerHTML = '<div class="container-fluid bloque3"><div class="col-md-10"><form name = "formTel2"><input type="number" id="telefono" class="form-control" placeholder="Telefono" ng-model="registroTel"  oninput="largoAdecuadoTel()" required/></form ></div ></form><div class="col-md-1"><button class="btn btn-danger btn-sm" type="button" onclick="remove_tels(' + nombreTels + ');"> <span class="glyphicon glyphicon-minus" aria-hidden="true"></span></button></div></div>';
    objTo.appendChild(divtest);
    document.getElementById('botonMasTel').disabled = true;
}
function remove_tels(rid) {//elimina los formularios correspondientes
    $('.removeclassTel' + rid).remove();
    cantidadTels--;
    largoAdecuadoTel();
}

function largoAdecuadoTel() {//valida si el numero ingresado tiene 8 digitos

    var input = document.getElementById('tels').firstElementChild;
    if (document.getElementById('InputTelefono').value.length >= 8) {
        document.getElementById('InputTelefono').value = document.getElementById('InputTelefono').value.slice(0, 8);//no permite ingresar mas digitos
        if (cantidadTels > 1) {
            while (input !== null) {//iteracion para validar todos los formularios de telefono
                var input2 = input.firstElementChild.firstElementChild.firstElementChild.firstElementChild;
                if (input2.value.length >= 8) {
                    input2.value = input2.value.slice(0, 8);
                    document.getElementById('botonMasTel').disabled = false;//enciende el boton si todos los campos tienen 8 digitos
                }
                else
                    document.getElementById('botonMasTel').disabled = true;//apaga el boton 
                input = input.nextElementSibling;
            }
        }
        else {
            document.getElementById('botonMasTel').disabled = false;
        }
    }
    else
        document.getElementById('botonMasTel').disabled = true;
}
/*----------------------------Fin del bloque de funciones ---------------------------------*/
function largoCedula(valor) {
    if (valor.value.length >= 9)
        valor.value = valor.value.slice(0, 9);
}