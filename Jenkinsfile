pipeline {
       stages {
         stage('Restore nuget') {
            steps {
              
              
                bat 'dotnet restore' //for .NET core
                bat 'nuget restore Crud.Demo.Web.Api.sln' // for .NET framework
               
            }
        }
        stage('Build') {
            steps {
               
                bat 'dotnet build --configuration Release ./WebApiWithMvcView/Crud.Demo.Web.Api/Api/Api.csproj'           
                bat 'msbuild Crud.Demo.Web.Api.sln /target:BigProject_NetFrameworkApp /p:Configuration=Release'
               
            }
        }
        stage('Test') {
            steps {
                bat 'dotnet test --logger trx ./WebApiWithMvcView/UnitTest/UnitTest.csproj'
            }
             post {
                    always {
                      //plugin: https://plugins.jenkins.io/mstest/
                        mstest testResultsFile:"**/*.trx", keepLongStdio: true
                    }
        
                }           
        }
        
        
    }
}