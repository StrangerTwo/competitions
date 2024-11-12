package com.example.module

import io.ktor.server.application.*
import io.ktor.server.plugins.callloging.*
import io.ktor.server.request.*

fun Application.configureLogging() {
    install(CallLogging) {
        format { call ->
            val status = call.response.status()
            val httpMethod = call.request.httpMethod.value
            val uri = call.request.uri
            val time = call.processingTimeMillis()
            val userAgent = call.request.headers["User-Agent"]
            val ip = call.request.headers["X-Forwarded-For"];

            "$httpMethod $uri ($status) in ${time}ms, From $ip, User agent: $userAgent"
        }
    }
}