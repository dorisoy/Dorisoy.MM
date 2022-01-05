app.controller("cRegistration", ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    //catch access token and user name    

    var tokenKey = 'accessToken';
    $scope.data = {};

    $scope.clearData = function (token) {
        $scope.data = {};
        $scope.detailsData = {};
    };

    $scope.registrationData = function (data) {
        $scope.$broadcast('show-errors-event');        
       
        if ($scope.newForm.$invalid) {
            swal(
                'Incomplete Form',
                'Please input required data.',
                'error'
              );
            return;
        }
        if (data.Password != data.ConfirmPassword) {
            swal(
                '',
                'These passwords do not match',
                'error'
              );
            return;
        }

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        // alert(data.Email + ' register called ');
        // $scope.isLoadingHide = false;

        $scope.message = "";
        $scope.showErrorMessage = "";
        document.body.style.cursor = 'wait';
        url = '../api/Registration/Register';
        $http({
            method: 'POST',
            url: url,
            data: data,
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                document.body.style.cursor = 'default';
                //$scope.message = "You have successfully registered.A confirmation link has been sent to your email address.";
                $scope.clearData();
                swal(
                    'Successfully Registered',
                    'A confirmation link has been sent to your email address',
                    'success'
                  ).then((value) => {
                      document.body.style.cursor = 'wait';
                      window.location.href = response.data;
                  });
                // alert("You have successfully registered.A confirmation link has been sent to your email address.");
                $scope.clearData();
            }
        }, function (response) {
            document.body.style.cursor = 'default';
            var obj = response.data;
            //alert(obj + " error");
            if (obj == "duplicate_email") {
                //$scope.showErrorMessage = "Email ID is already Taken.";
                swal(
                    'Error',
                    'Email ID is already Taken',
                    'error'
                  );
            } else if (obj == "User Name is already Taken") {
                swal(
                  'Sorry!',
                  'User Name is already Taken',
                  'error'
                 );
            }
            else {
                swal(
                  'Sorry!',
                  'Registration Failed! Please Insert Your Information Correctly',
                  'error'
                 );
            }
        });
    };

}]);
