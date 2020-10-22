mkdir compiled
protoc.exe echo.proto --js_out=import_style=commonjs:compiled --grpc-web_out=import_style=commonjs,mode=grpcwebtext:compiled
pause