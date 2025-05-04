module Demo.Main
open Browser

/// Create an element that displays a temperature conversion example
let createExampleElement (example: {| InputValue: float; InputUnit: string |}) =
    let div = document.createElement "div"
    div.className <- "example"
    
    // HERE IS WHERE THE MAGIC HAPPENS AND WE GET TO USE THE TEMPERATURE MODULE
    // We're using a tuple to return the formula, the conversion function, and the result unit
    let (formula, convert, resultUnit) = 
        if example.InputUnit = "°C" then 
            ("C → F:", GlueTemplate.Temperature.celsiusToFahrenheit, "°F")
        else 
            ("F → C:", GlueTemplate.Temperature.fahrenheitToCelsius, "°C")
    
    // Set the HTML content with the conversion calculation
    div.innerHTML <- $"""
        <strong>{formula}</strong><br>
        {example.InputValue}{example.InputUnit} = {convert example.InputValue}{resultUnit}
    """
    
    div

/// Initialize the demo by populating the examples section
let initialize() =
    let examples = document.getElementById "examples"

    let data = [|
        {| InputValue = 37.0; InputUnit = "°C" |}
        {| InputValue = 98.6; InputUnit = "°F" |}
    |]
    
    data
    |> Array.map createExampleElement
    |> Array.iter (fun element -> examples.appendChild element |> ignore)

// Start the application
initialize() 