VAR decided = "null"
-> main

=== main ===
Hello Stranger, Would you like to give a poor man some of your money please?
    * [Yeah sure, why not?]
        Thank you so much little guy!
        ~ decided = "yes"
        -> DONE

    * [Hell nah man]
        "The man stares at you with open-wide eyes..."
        ~ decided = "no"
        -> DONE
        
-> END