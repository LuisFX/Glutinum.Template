/// Temperature conversion utilities.
module GlueTemplate.Temperature

/// Convert Celsius to Fahrenheit
/// Formula: F = C * 9/5 + 32
let celsiusToFahrenheit (celsius: float) : float =
    // Simple conversion formula
    celsius * 9.0 / 5.0 + 32.0

/// Convert Fahrenheit to Celsius
/// Formula: C = (F - 32) * 5/9
let fahrenheitToCelsius (fahrenheit: float) : float =
    // Simple conversion formula
    (fahrenheit - 32.0) * 5.0 / 9.0 