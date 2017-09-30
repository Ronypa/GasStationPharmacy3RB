var url = "/ApiWeb/api/login"
var app = angular.module("app", []);

var MainController = function ($scope, $http) {

    var onSucess = function (response) { $scope.users = response.data };

    var onFailure = function (reason) { $scope.error = reason };

    var getUsers = function () {
        $http.get(url)
            .then(onSucess, onFailure)
    };
    getUsers();
};

app.controller("MainController", MainController);
