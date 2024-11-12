package com.example.util

import java.time.LocalDateTime
import java.time.LocalTime
import java.time.ZoneId
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter
import java.time.format.DateTimeFormatterBuilder
import java.time.temporal.ChronoField

fun String.toLocalDateTime(): LocalDateTime {
    return LocalDateTime.parse(
        this, DateTimeFormatterBuilder()
            .appendPattern("yyyy-MM-dd HH:mm:ss")
            .appendFraction(ChronoField.MICRO_OF_SECOND, 0, 3, true)
            .toFormatter().withZone(ZoneOffset.UTC)
    )
}

fun String.toLocalTime(pattern: String): LocalTime {
    return LocalTime.parse(
        this, DateTimeFormatter.ofPattern(pattern).withZone(ZoneId.of("Europe/Prague"))
    )
}

fun LocalTime.toString(pattern: String): String {
    return this.format(DateTimeFormatter.ofPattern(pattern).withZone(ZoneId.of("Europe/Prague")))
}