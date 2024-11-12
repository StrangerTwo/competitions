package com.example.module

import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.server.routing.*

fun Application.configureRouting() {
    val target = System.getenv("TARGET") ?: "World"

    routing {
        get("/") {
            call.respondText("Hello, $target!", ContentType.Text.Html)
        }
        get("{...}") {
            call.respondText("Hello from ${call.request.uri}", ContentType.Text.Html)
        }
    }
}