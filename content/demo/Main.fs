module Demo.Main

open Browser
open Browser.Types
open GlueTemplate.Temperature

/// Create a conversion row with inputs and conversion function
let createConverter (label1: string, label2: string, convertFn: float -> float) =
    // Create container
    let container = document.createElement "div"
    container.className <- "grid grid-cols-[1fr_50px_1fr] gap-2 items-center mb-5 bg-white p-5 rounded-lg shadow-sm"
    
    // Create input for first temperature
    let input1 = document.createElement "input" :?> HTMLInputElement
    input1.``type`` <- "number"
    input1.placeholder <- label1
    input1.step <- "0.1"  // Allow decimal inputs
    input1.className <- "w-full p-2 border border-gray-300 rounded text-base focus:outline-none focus:border-blue-500"
    
    // Add a small label
    let small1 = document.createElement "small"
    small1.textContent <- label1
    small1.className <- "block text-gray-500 mt-1"
    let div1 = document.createElement "div"
    div1.appendChild(input1) |> ignore
    div1.appendChild(small1) |> ignore
    
    // Add to container
    container.appendChild(div1) |> ignore
    
    // Add arrow
    let arrow = document.createElement "div"
    arrow.className <- "text-center font-bold text-blue-500"
    arrow.innerHTML <- "→"
    container.appendChild(arrow) |> ignore
    
    // Create input for second temperature
    let input2 = document.createElement "input" :?> HTMLInputElement
    input2.``type`` <- "number"
    input2.placeholder <- label2
    input2.readOnly <- true
    input2.className <- "w-full p-2 border border-gray-300 rounded text-base bg-gray-50"
    
    // Add a small label
    let small2 = document.createElement "small"
    small2.textContent <- label2
    small2.className <- "block text-gray-500 mt-1"
    let div2 = document.createElement "div"
    div2.appendChild(input2) |> ignore
    div2.appendChild(small2) |> ignore
    
    // Add to container
    container.appendChild(div2) |> ignore
    
    // Add event listener for input changes
    input1.addEventListener("input", fun _ ->
        // Update the second input when the first input changes
        if input1.value <> "" then
            try
                let value = float input1.value
                let result = convertFn value
                input2.value <- result.ToString("0.##")  // Format to 2 decimal places
            with _ ->
                input2.value <- ""
        else
            input2.value <- ""
    )
    
    // Return the container
    container

/// Initialize the demo
let initializeDemo() =
    // Create main container
    let container = document.createElement "div"
    container.className <- "max-w-lg mx-auto my-10 p-6 bg-gray-50"
    document.body.appendChild(container) |> ignore
    
    // Add title
    let title = document.createElement "h1"
    title.textContent <- "Temperature Converter"
    title.className <- "text-3xl text-center text-gray-800 mb-8 font-semibold"
    container.appendChild(title) |> ignore
    
    // Create converters
    let celsiusToFahrenheitConverter = 
        createConverter("Celsius", "Fahrenheit", celsiusToFahrenheit)
    container.appendChild(celsiusToFahrenheitConverter) |> ignore
    
    let fahrenheitToCelsiusConverter = 
        createConverter("Fahrenheit", "Celsius", fahrenheitToCelsius)
    container.appendChild(fahrenheitToCelsiusConverter) |> ignore
    
    // Add a note explaining the app
    let note = document.createElement "div"
    note.className <- "mt-8 p-4 bg-blue-50 border-l-4 border-blue-500 rounded text-sm text-gray-700 leading-relaxed"
    note.innerHTML <- """
        <strong>About this demo:</strong>
        <p class="mt-2">This simple temperature converter demonstrates how to create F# bindings with Fable.
        It uses Tailwind CSS through CDN and shows the basic pattern of creating type-safe F# functions
        that can be easily used from JavaScript.</p>
        <p class="mt-2">The converter functions are located in the GlueTemplate.Temperature module.</p>
    """
    container.appendChild(note) |> ignore

// Start the application
initializeDemo()

