app.controller("global", ['$scope', '$http', '$rootScope', '$location', function ($scope, $http, $rootScope, $location) {

    //catch access token and user name
    var tokenKey = 'accessToken';
    $scope.data = {};
    $scope.img = "default";

    $scope.GetProfileImg = function () {
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        urlData = '../api/Meeting';
        $http({
            method: 'GET',
            url: urlData,
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                var char = response.data.USERDATA[0].UserName.charAt(0);
                $scope.img = char;
            }
        }, function (response) {
            console.log(response);
        });
    };


    //change password
    $scope.changePass = function (data) {
        $scope.$broadcast('show-errors-event');

        if ($scope.newForm.$invalid) {
            swal(
                'Incomplete Form',
                'Please input required data.',
                'error'
              );
            return;
        }
        else {
            var token = sessionStorage.getItem(tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            urlData = '../api/Logout/ChangePassword';
            document.body.style.cursor = 'wait';
            $http({
                method: 'POST',
                url: urlData,
                headers: headers,
                data: data,
            }).then(function (response) {
                if (response.status == 200) {
                    swal(
                       'Successfully Changed Password',
                       '',
                       'success'
                     ).then((value) => {
                         window.location.href = "UserProfile";
                     });
                }
            }, function (response) {
                document.body.style.cursor = 'default';
                if (response.status == 400) {
                    swal(
                        'Error',
                        response.data.ModelState.error[0],
                        'error'
                      );
                }
            });
        }
    };

    //change email
    $scope.changeEmail = function (data) {

        var token = sessionStorage.getItem(tokenKey);
        //alert(token);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        urlData = '../api/Registration/ChangeEmail';
        document.body.style.cursor = 'wait';
        $http({
            method: 'POST',
            url: urlData,
            headers: headers,
            data: data,
        }).then(function (response) {
            if (response.status == 200) {
                swal(
                        'Successfully Update',
                        'Information Update Successfully',
                        'success'
                      ).then((value) => {
                          window.location.href = response.data;
                      });
            }
        }, function (response) {
            document.body.style.cursor = 'default';
            if (response.status == 400) {
                swal(
                    'Error',
                    response.data.ModelState.error[0],
                    'error'
                  );
            }
            //  alert('Sorry!');
            //console.log(response);
        });
    };

    //Logout
    $scope.logout = function () {

        var token = sessionStorage.getItem(tokenKey);
        //alert(token);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        urlLogoutData = '../api/Logout/Logout';
        $http({
            method: 'POST',
            url: urlLogoutData,
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                sessionStorage.removeItem(tokenKey);
                sessionStorage.clear();
                window.location.href = response.data;
            }
        }, function (response) {
            //  alert('Sorry!');
            console.log(response);
        });
    };
}]);


