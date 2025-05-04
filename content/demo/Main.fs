module Demo.Main

open Browser

let h1 = document.createElement "h1"
h1.innerHTML <- GlueTemplate.Exports.helloWorld
document.body.appendChild h1 |> ignore

let hr = document.createElement "hr"
document.body.appendChild hr |> ignore

let div2 = document.createElement "div"
div2.innerHTML <- GlueTemplate.Exports.welcomeMessage
document.body.appendChild div2 |> ignore

