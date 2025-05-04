module Demo.Main

open Browser
open Browser.Types
open GlueTemplate.Temperature

/// CSS styles using Tailwind classes.
module Styles =
    /// Main container style
    let container = "max-w-lg mx-auto my-10 p-6 bg-gray-50"
    
    /// Title style
    let title = "text-3xl text-center text-gray-800 mb-8 font-semibold"
    
    /// Converter container with grid layout
    let converterContainer = "grid grid-cols-[1fr_50px_1fr] gap-4 items-center mb-5 bg-white p-5 rounded-lg shadow-sm"
    
    /// Input field style
    let input = "w-full p-2 border border-gray-300 rounded text-base focus:outline-none focus:border-blue-500"
    
    /// Small label beneath inputs
    let label = "block text-gray-500 mt-1"
    
    /// Bidirectional arrow style
    let arrow = "text-center font-bold text-blue-500"
    
    /// Info note box style
    let note = "mt-8 p-4 bg-blue-50 border-l-4 border-blue-500 rounded text-sm text-gray-700 leading-relaxed"
    
    /// Paragraph style in the note
    let noteParagraph = "mt-2"

/// Helper functions for creating UI elements
module Helpers =
    /// Create an input field with a label
    let createLabeledInput (labelText: string) =
        // Create input
        let input = document.createElement "input" :?> HTMLInputElement
        input.``type`` <- "number"
        input.placeholder <- labelText
        input.step <- "0.1"
        input.className <- Styles.input
        
        // Create label
        let label = document.createElement "small"
        label.textContent <- labelText
        label.className <- Styles.label
        
        // Create container div
        let container = document.createElement "div"
        container.appendChild(input) |> ignore
        container.appendChild(label) |> ignore
        
        // Return both the container and the input
        (container, input)
    
    /// Setup bidirectional conversion between two temperature inputs
    let setupConversion (celsiusInput: HTMLInputElement) (fahrenheitInput: HTMLInputElement) =
        // Flag to prevent infinite update loop
        let mutable isUpdating = false
        
        // Add event listener for Celsius input changes
        celsiusInput.addEventListener("input", fun _ ->
            if not isUpdating && celsiusInput.value <> "" then
                isUpdating <- true
                try
                    let celsius = float celsiusInput.value
                    let fahrenheit = celsiusToFahrenheit celsius
                    fahrenheitInput.value <- fahrenheit.ToString("0.##")
                with _ ->
                    fahrenheitInput.value <- ""
                isUpdating <- false
            elif celsiusInput.value = "" then
                fahrenheitInput.value <- ""
        )
        
        // Add event listener for Fahrenheit input changes
        fahrenheitInput.addEventListener("input", fun _ ->
            if not isUpdating && fahrenheitInput.value <> "" then
                isUpdating <- true
                try
                    let fahrenheit = float fahrenheitInput.value
                    let celsius = fahrenheitToCelsius fahrenheit
                    celsiusInput.value <- celsius.ToString("0.##")
                with _ ->
                    celsiusInput.value <- ""
                isUpdating <- false
            elif fahrenheitInput.value = "" then
                celsiusInput.value <- ""
        )

/// Initialize the demo
let initializeDemo() =
    // Create main container
    let container = document.createElement "div"
    container.className <- Styles.container
    document.body.appendChild(container) |> ignore
    
    // Add title
    let title = document.createElement "h1"
    title.textContent <- "Temperature Converter"
    title.className <- Styles.title
    container.appendChild(title) |> ignore
    
    // Create bidirectional converter
    let converterContainer = document.createElement "div"
    converterContainer.className <- Styles.converterContainer
    container.appendChild(converterContainer) |> ignore
    
    // Create input fields with labels
    let (celsiusDiv, celsiusInput) = Helpers.createLabeledInput "Celsius"
    let (fahrenheitDiv, fahrenheitInput) = Helpers.createLabeledInput "Fahrenheit"
    
    // Add to container
    converterContainer.appendChild(celsiusDiv) |> ignore
    
    // Add bidirectional arrow
    let arrow = document.createElement "div"
    arrow.className <- Styles.arrow
    arrow.innerHTML <- "↔"
    converterContainer.appendChild(arrow) |> ignore
    
    // Add fahrenheit input to container
    converterContainer.appendChild(fahrenheitDiv) |> ignore
    
    // Set up conversion between the inputs
    Helpers.setupConversion celsiusInput fahrenheitInput
    
    // Add a note explaining the app
    let note = document.createElement "div"
    note.className <- Styles.note
    note.innerHTML <- """
        <strong>About this demo:</strong>
        <p class="mt-2">This simple temperature converter demonstrates how to create F# bindings with Fable.
        It uses Tailwind CSS through CDN and shows the basic pattern of creating fable bindings/packages.</p>
        <p class="mt-2">The converter functions are located in the GlueTemplate.Temperature module.</p>
    """
    container.appendChild(note) |> ignore

// Start the application
initializeDemo()

