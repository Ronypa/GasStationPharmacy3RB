app.factory('authFact', ["$cookieStore",function ($cookieStore) {
    var authFact = {};
    authFact.setAccessToken = function (accessToken) {
        $cookieStore.put('accessToken', accessToken);
    };

    authFact.getAccessToken = function () {
        authFact.authToken = $cookieStore.get('accessToken');
        return authFact.authToken;
    };

    return authFact;
}]);