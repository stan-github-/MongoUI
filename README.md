MongoUI
=======

MongoUI is a bear-bone mongo mananger modeled after sql server manager, where multiple queries and query results are handled in separate windows.  

Core features:
1) Opens queries in separate windows.
2) Switches easily amongst servers and databases.
3) Outputs results to desired exe (notepad) or inside a pane.
4) Allows inclusion of libraries such as Underscore.js.

Additional features:
1) Pops confirmation dialog before query execution, depending on server configuration.
2) Reopens all files opened in previous session.
3) Stores static list of files for quick access.

Features in progress:
1) Autocomplete (implemented "db.collection." and "db.collection.find()")
2) Executes DOS and SQL cmds 
  a) separated by "##MONGO##", "##DOS##", or "##SQL##"
  b) does not support sql server switching based on environment.
