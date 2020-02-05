# SearchActiveUserAD
With this tool you can search active directory users from txt file

With this tool you can search active users in active directory from txt file, for example,
you have this users in the txt file:
  
  CALDERON  CHACON MARIA PAULA <txt line>
  GONZALEZ  FONSECA JORGE ARTURO <txt line>
  MORA  VILLALOBOS CRISTEL MARIANA <txt line>
  BENAVIDES  SOLANO GREIVIN GERARDO <txt line>
  CAMPOS ALFARO KEVIN <txt line>
  BENAVIDES GONZALEZ EDUARDO <txt line>
  JIMENEZ ARAYA RAUL <txt line>
  FERNANDEZ MONTERO RICARDO ANDRES <txt line>
  HERNANDEZ MUÑOZ MERLYN CLEER <txt line>
  MORA  ALVARADO JOSE ROLANDO <txt line>
  GAMBOA  PERAZA MICHAEL FERNANDO <txt line>
  MORA  LAMMAS MILENA <txt line>
  ZAMORA  ROJAS VICTOR HUGO <txt line>
  AGUERO  MENDOZA  JOSE JOAQUIN <txt line>
  FONSECA  ARAYA  CARLOS <txt line>
  SALAZAR  CALDERON LUIS FERNANDO <txt line>

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
