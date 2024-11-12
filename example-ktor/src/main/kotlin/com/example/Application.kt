package com.example

import com.example.module.*
import io.ktor.server.application.*
import io.ktor.server.engine.*
import io.ktor.server.netty.*

fun main() {
    val port = System.getenv("PORT") ?: "8080"
    embeddedServer(Netty, port = port.toInt(), host = "0.0.0.0", module = Application::module)
        .start(wait = true)
}


fun Application.module() {
    configureSerialization()
    configureLogging()
    configureDatabases()
    configureRouting()

}
