# MicroFrontEndWithMicroServices
This repository is related to application example for Micro Front End and Micro Services concepts together

# Tools and Technologies 
    * Visual Studio 2019 Community
    * Net Core 2.1
    * Postman latest version
    * MySQL Version 8.0.19
    * Github

# Git Commands
    * git add .
    * git commit -m "message"

# Test Service in PostMan
    * Open PostMan
    * Place route url for api
        - Generally it will be api/controller/action/optionalParameter
    * If Request Type is Get then
        - Select Authorization
        - Type = Bearer Token
        - Paste Token
        - Send
    * If Request Type is Post then
        - Select Body
        - Select raw
        - Select JSOM from last drop down
        - Give all argument in JSON format
        - Click Send

# Hashing & Salting
    * Hashing is one way transformation of message or any value to fixed size bit string
    * It means it convert any length string to one fixed size bit string using mathematical formula
    * It is one way because you cannot convert back bit string to regular message.
    * This one way conversation gives advantage over any other decryption algorithm.
    * Salt is random bit array which will be added to message/password before performing hashing.
    * Salt gives more layer of security to hashing to avoid attacks like "Rainbow Table".
    * Salt needs to be random similar kind of salt cannot help you to increase security.
    * In Case of Random SALT + HASH will make your message/password much secure than any other formulas.
    * https://en.wikipedia.org/wiki/Cryptographic_hash_function
    * https://stackoverflow.com/questions/1645161/salt-generation-and-open-source-software/1645190#1645190
