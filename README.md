# zendesk
This project was built using Visual Studio Professional 2015 on .Net Framework 4.6.1. It references the Newtonsoft.Json library for the Json serialization.

A Runtime folder has been included that can be run without opening Visual Studio.

The Json files are located in the Data folder.

The following commands are available from the console:
quit
help
select  search for items using the following syntax
        select [fields] from [table] [where [field]=[value]]
                fields must be * or a comma delimited list of fields in the table
                table must be a single table name from the searchable list of tables
                where is optional, but if specified, must be fieldname = or < or > value
        select * from Users where _id=71
        select * from Tickets where status="hold"
        select _id,name,organization from Users where verified=true
        select _id,name,verified from Users
        select _id,name,details,tags from Organizations where created_at>2016-05-05
        Valid operators are:
                = (for all types)
                < (for int and DateTime types)
                > (for int and DateTime types)
fields  view a list of searchable fields
        Optionally pass a table name to only show fields for that table
        fields Users
tables  view a list of searchable tables