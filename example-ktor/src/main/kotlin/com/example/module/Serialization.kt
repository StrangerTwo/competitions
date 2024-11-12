package com.example.module

import com.example.util.serializer.LocalDateSerializer
import com.example.util.serializer.LocalDateTimeSerializer
import com.example.util.serializer.LocalTimeSerializer
import io.ktor.serialization.kotlinx.json.*
import io.ktor.server.application.*
import io.ktor.server.plugins.contentnegotiation.*
import kotlinx.serialization.json.Json
import kotlinx.serialization.modules.SerializersModule
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.LocalTime

fun Application.configureSerialization() {
    install(ContentNegotiation) {
        json(Json {
            serializersModule = SerializersModule {
                contextual(LocalDate::class, LocalDateSerializer)
                contextual(LocalDateTime::class, LocalDateTimeSerializer)
                contextual(LocalTime::class, LocalTimeSerializer)
            }
        })
    }
}