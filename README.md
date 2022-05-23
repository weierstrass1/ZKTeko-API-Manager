# ZKTeko-API-Manager

Este proyecto tiene como finalidad el envio de marcas desde la Base de datos de ZKTeco a la API de CTRL.

## Settings

Incluye un archivo settings.json similar al siguiente:

```
{
  "DBServerName": "*****",
  "DBServerIP": "*****",
  "DBUsername": "*****",
  "DBPassword": "*****",
  "APIUrlReSend": "????",
  "APIUrlHour": "????",
  "APISuccessMessage": "Exito",
  "DBIgnoreMessages": "('Exito','duplicado','Reenviado')",
  "Days": 0,
  "SerialsNumber": "('CJIK220760004')",
  "MaxDiffTime": 5,
  //Si DateRange es null, usa Days sino usa Range
  "DateRange": null,
  //"DateRange": [ "2022-05-18T00:00:00", "2022-05-19T00:00:00" ],
  "ReSendStatus": "Reenviado",
  "SendLog": false
}
```
  
- DBServerName: Nombre de la base de datos.
- DBServerIP: IP de la base de datos.
- DBUsername: Usuario de la base de datos.
- DBPassword: Password de la base de datos.
- APIUrlReSend: Direccion URL de la API de CTRL para recibir registros de la base de datos.
- APIUrlHour: Direccion URL de la API de CTRL para obtener los millisegundos de la aplicacion de CTRL.
- APISuccessMessage: Mensaje de exito que debiera recibir desde la API de CTRL.
- DBIgnoreMessages: Los registros de la base de datos que tengan como respuesta alguno de los mensajes de esta lista, seran ignorados cuando se utilice el comando --ignore.
- Days: Cantidad de dias para revisar en la base de datos, 0 es el dia actual.
- SerialsNumber: Numeros seriales de dispositivos que se utilizan para filtrar cuando se usa el comando --serial.
- MaxDiffTime: Cantidad de tiempo que se considera valido entre la diferencia de la plataforma y el servidor.
- DateRange: Rango de tiempo que se usa para envio de registros, si es null se utiliza el parametro days, si no es null se utiliza el rango de tiempo.
- ReSendStatus: Respuesta que se pone en la base de datos al reenviar un mensaje.
- SendLog: Si es true, se generara un archivo log con informacion de utilidad.

## Opciones

- --ignore : Al usarlo, se filtraran los registros de la base utilizando solo aquellos que no tengan una respuesta incluida en el setting DBIgnoreMessages.
- --hour: Al usarlo, te dara la hora de la plataforma de CTRL y de el servidor de la DB en UTC.
- --serial: Al usarlo, se filtraran los registros para que solo sean de dispositivos en la lista del setting SerialsNumber.
