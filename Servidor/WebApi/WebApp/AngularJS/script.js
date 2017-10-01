var app = angular.module('app', ['ngRoute']);

//Ruteador, (relaciona direccion con html y controlador)
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/AngularJS/Templates/login.html',
        controller: 'logincontroller'
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