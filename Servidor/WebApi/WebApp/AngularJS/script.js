var app = angular.module('app', ['ngRoute']);

//Ruteador, (relaciona direccion con html y controlador)
app.config(function ($routeProvider) {
    $routeProvider.when('/e', {
        templateUrl: '/AngularJS/Templates/login.html',
        controller: 'logincontroller'
    })
    .otherwise({
        redirectTo: '/'
        });

    $routeProvider.when('/', {
        templateUrl: '/AngularJS/Templates/entra.html',
        controller: 'entracontroller'
    })
        .otherwise({
            redirectTo: '/'
        });

});

//Servicio para realizar peticiones http 
app.service('loginservice', function ($http) {

    this.login = function (userlogin) {
        var resp = $http({
            url: "/WebApi/token",
            method: "POST",
            data: $.param({ grant_type: 'password', userName: userlogin.username, password: userlogin.password }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        });
        return resp;
    };
});

//Servicio para realizar peticiones http 
app.service('crearservice', function ($http) {

    this.crear = function (usercrear) {
        var resp = $http({
            url: "api/EMPLEADO",
            method: "POST",
            data: $.param({
                Cedula: usercrear.cedula,
                Nombre1: usercrear.nombre1,
                Nombre2: usercrear.nombre2,
                Apellido1: usercrear.apellido1,
                Apellido2: usercrear.apellido2,
                Provincia: usercrear.provincia,
                Cuidad: usercrear.cuidad,
                Señas: usercrear.señas,
                FechaNaciento: usercrear.fechaNacimiento,
                Contraseña: usercrear.contraseña,
                Sucursal: usercrear.sucursal,
                Activo: usercrear.activo
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        });
        return resp;
    };
});


app.controller('crearcontroller', function ($scope, crearservice) {

    //Funcion para hacer login 
    $scope.crear = function () {
        //Body de la peticion http
        var userCrear = {
            cedula: $scope.cedula,
            nombre1: $scope.nombre1,
            nombre2: $scope.nombre2,
            apellido1: $scope.apellido1,
            apellido2: $scope.apellido2,
            provincia: $scope.provincia,
            cuidad: $scope.cuidad,
            señas: $scope.señas,
            fechaNacimiento: $scope.fechaNacimiento,
            contraseña: $scope.contraseña,
            sucursal: $scope.sucursal,
            activo: $scope.activo,

        };
        //Hace uso del servicio y ejecuta la peticion http
        var promisecrear = crearservice.crear(userCrear);
        promisecrear.then(function (resp) {//Si se ejecuta la peticion bien
            alert("Bien");
        }, function (err) {//Si la peticion no funciono bien
            alert("ERROR NO SEA ESTUPIDO .l.");
        });
    };
});




//Controlador para el login tiene la funcion que se llama desde el html (hace uso del servicio "loginService")
app.controller('logincontroller', function ($scope, loginservice) {
    
    //Funcion para hacer login 
    $scope.login = function () {
        //Body de la peticion http
        var userLogin = {
            grant_type: 'password',//Solo para login
            username: $scope.userLoginEmail,
            password: $scope.userLoginPassword
        };
        //Hace uso del servicio y ejecuta la peticion http
        var promiselogin = loginservice.login(userLogin);
        promiselogin.then(function (resp) {//Si se ejecuta la peticion bien
            //Guarda parte de la respuesta del servicio
            $scope.userName = resp.data.userName;
            sessionStorage.setItem('userName', resp.data.userName);
            sessionStorage.setItem('accessToken', resp.data.access_token);
            window.location.href = '/AngularJS/Templates/entra.html';//Redirecciona la pagina
        }, function (err) {//Si la peticion no funciono bien
            alert("ERROR NO SEA ESTUPIDO .l.");
            $scope.responseData = "Error " + err.status;
        });
    };
});