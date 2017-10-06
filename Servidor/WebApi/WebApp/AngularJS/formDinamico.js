

var room = 1;
function dinamicoPade() {
    room++;
    var objTo = document.getElementById('dinamico');
    var divtest = document.createElement("div");
    divtest.setAttribute("class", "form-group removeclass" + room);
    var rdiv = 'removeclass' + room;
    divtest.innerHTML = '<div class="col-md-5"><form><input type="text" class="form-control" placeholder="Padecimiento" ng-model="registroPadecimimeto" /></form></div><div class="col-md-5"><form"><input type="text" onfocus="(this.type=\'date\')" class="form-control" placeholder="Fecha de pronóstico" ng-model="registroPadecimimetoFecha" /></form></div><button class="btn btn-danger btn-sm" type="button" onclick="remove_pades(' + room + ');"> <span class="glyphicon glyphicon-minus" aria-hidden="true"></span> </button></form >';
    objTo.appendChild(divtest)
}
function remove_pades(rid) {
    $('.removeclass' + rid).remove();
    room--;
}


var room2 = 1;
var tell2 = "nada";
function dinamicoTel() {
    var tell = document.getElementById("telefono").value;
    
    if ((tell != "") && (tell2 !="")) {
        room2++;
        var objTo = document.getElementById('tels')
        var divtest = document.createElement("div");
        divtest.setAttribute("class", "form-group removeclass" + room2);
        var rdiv = 'removeclass' + room2;
        divtest.innerHTML = '<div class="container-fluid bloque3"><div class="col-md-10"><form><input type="number" id="telefono2" class="form-control" placeholder="Telefono" ng-model="registroTel" /></form ></div ></form><div class="col-md-1"><button class="btn btn-danger btn-sm" type="button" onclick="remove_tels(' + room2 + ');"> <span class="glyphicon glyphicon-minus" aria-hidden="true"></span></button></div></div>';
        objTo.appendChild(divtest)
        tell2 = document.getElementById("telefono2").value;
    }
}
function remove_tels(rid) {
    $('.removeclass' + rid).remove();
}