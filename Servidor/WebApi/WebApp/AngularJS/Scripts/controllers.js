//Controlador para el login tiene la funcion que se llama desde el html (hace uso del servicio "loginService")
app.controller('logincontroller', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {

    $scope.loginAdministrador = function () {
        var empleado =
            "userName=" + encodeURIComponent($scope.loginCedula) +
            "&password=" + encodeURIComponent($scope.loginPassword) +
            "&grant_type=password" +
            "&client_id=empleado"
            ;

        var apiRoute = '/WebApi/token';
        var respuesta = ServicioHTTP.post(apiRoute, empleado);
        respuesta.then(function (response) {

            var accessToken = response.data.access_token;
            authFact.setAccessToken(accessToken);
            $location.path("/entro");
        },
            function (error) {
                authFact.setAccessToken("");
                alert("Usuario y/o contraseña incorrecto");
            }
        );
    };

    $scope.loginCliente = function () {
        var cliente =
            "userName=" + encodeURIComponent($scope.loginCedula) +
            "&password=" + encodeURIComponent($scope.loginPassword) +
            "&grant_type=password" +
            "&client_id=cliente"
            ;

        var apiRoute = '/WebApi/token';
        var respuesta = ServicioHTTP.post(apiRoute, cliente);
        respuesta.then(function (response) {

            var accessToken = response.data.access_token;
            authFact.setAccessToken(accessToken);
            $location.path("/entro");
        },
            function (error) {
                authFact.setAccessToken("");
                alert("Usuario y/o contraseña incorrecto");
            }
        );
    };

    //funciones para redireccionar a las vistas de log in
    $scope.loginadministrador = function () {
        $location.path("/loginadministrador");
    };
    $scope.logincliente = function () {
        $location.path("/logincliente");
    };

});