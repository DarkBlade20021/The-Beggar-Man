VAR decided = "null"
-> main

=== main ===
Hello young man, Would you like to give a poor beggar man some of your money please?
    * [Yeah sure, why not?]
        Thank you so much little guy!
        ~ decided = "yes"
        -> DONE

    * [Hell nah man]
        "The man stares at you with open-wide eyes..."
        ~ decided = "no"
        -> DONE
        
-> END