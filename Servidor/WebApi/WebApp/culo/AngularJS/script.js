var app = angular.module('app', ['ngRoute']);

//Ruteador, (relaciona direccion con html y controlador)
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/AngularJS/Templates/principal.html',
        controller: 'logincontroller'
    })
        .when("/logincliente", {
            templateUrl: '/AngularJS/Templates/loginCliente.html',
            controller: 'logincontroller'
        })
        .when("/loginadministrador", {
            templateUrl: "/AngularJS/Templates/loginAdministrador.html",
            controller: "logincontroller"
        })
    .otherwise({
        redirectTo: '/'
    });
});

//Servicio para realizar peticiones http 
app.service('loginservice', function ($http) {

    this.loginCliente = function (userlogin) {
        var resp = $http({
            url: "/WebApi/token",
            method: "POST",
            data: $.param({ grant_type: 'password', userName: userlogin.username, password: userlogin.password }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        });
        return resp;
    };
});


//Controlador para el login tiene la funcion que se llama desde el html (hace uso del servicio "loginService")
app.controller('logincontroller', function ($scope, loginservice,$location) {
    
    //Funcion para hacer login 
    $scope.loginCliente = function () {
        //Body de la peticion http
        var userLogin = {
            grant_type: 'password',//Solo para login
            username: $scope.loginCedula,
            password: $scope.loginPassword
        };
        //Hace uso del servicio y ejecuta la peticion http
        var promiselogin = loginservice.loginCliente(userLogin);
        promiselogin.then(function (resp) {//Si se ejecuta la peticion bien
            //Guarda parte de la respuesta del servicio
            $scope.userName = resp.data.userName;
            sessionStorage.setItem('userName', resp.data.userName);
            sessionStorage.setItem('accessToken', resp.data.access_token);
            window.location.href = '/AngularJS/Templates/entra.html';//Redirecciona la pagina
        }, function (err) {//Si la peticion no funciono bien
            alert("Cedula y/o contraseña incorrectas");
            $scope.responseData = "Error " + err.status;
        });
    };
    //funciones para redireccionar a las vistas de log in
    $scope.loginadministrador = function () {
        $location.path("/loginadministrador");
    };
    $scope.logincliente = function () {
        $location.path("/logincliente");
    };
});