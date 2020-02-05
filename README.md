# SearchActiveUserAD
With this tool you can search active directory users from txt file

With this tool you can search active users in active directory from txt file, for example,
you have this users in the txt file:
  
  CALDERON  CHACON MARIA PAULA
  GONZALEZ  FONSECA JORGE ARTURO
  MORA  VILLALOBOS CRISTEL MARIANA
  BENAVIDES  SOLANO GREIVIN GERARDO
  CAMPOS ALFARO KEVIN
  BENAVIDES GONZALEZ EDUARDO
  JIMENEZ ARAYA RAUL
  FERNANDEZ MONTERO RICARDO ANDRES
  HERNANDEZ MUÑOZ MERLYN CLEER
  MORA  ALVARADO JOSE ROLANDO
  GAMBOA  PERAZA MICHAEL FERNANDO
  MORA  LAMMAS MILENA
  ZAMORA  ROJAS VICTOR HUGO
  AGUERO  MENDOZA  JOSE JOAQUIN
  FONSECA  ARAYA  CARLOS
  SALAZAR  CALDERON LUIS FERNANDO

The tool will show you what user or users are active, will show the Windows account, user name, last name and job title. 
But in this format the users have the last name first and later the name, the tool will allowed you choose the format, 
with 'correct format' or 'incorrect format'.
for example:
  CORRECT FORMAT: MARIA PAULA CALDERON  CHACON
  INCORRECT FORMAT: CALDERON  CHACON MARIA PAULA
NOTE: The name MARIA PAULA CALDERON  CHACON has two spaces between each word, don't worry, the tool will remove this double spaces.

So, don't worry about the order name, if you need change this order, you will have to change the code 
in the method 'orderNames()' for incorrect format and 'orderNames_orderedList()' in the correct format.

IMPORTANT: This will need your credentials admin of Active Directory to search the active users in the txt file.
