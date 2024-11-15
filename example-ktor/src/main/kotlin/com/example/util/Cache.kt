package com.example.util

import java.time.Instant
import java.util.concurrent.locks.ReentrantLock
import kotlin.concurrent.withLock
import kotlin.time.Duration

data class CacheItem<T>(
    val value: T,
    val timeout: Instant?,
) {
    val isValid: Boolean
        get() = timeout == null || Instant.now().isBefore(timeout)
}

val cache = hashMapOf<String, CacheItem<out Any>>();
val generatorLocks = hashMapOf<String, ReentrantLock>();

inline fun <T : Any> cacheFor(key: String, duration: Duration? = null, generator: () -> T): T {
    generatorLocks
        .getOrPut(key) { ReentrantLock() }
        .withLock {
            val cachedItem = cache[key];

            if (cachedItem?.isValid == true) {
                return cachedItem.value as T
            }

            val result = generator();
            cache[key] = CacheItem(
                value = result,
                timeout = duration?.let { Instant.now().plusMillis(it.inWholeMilliseconds) }
            )
            return result;
        }
}

fun invalidateCache(key: String) {
    cache.remove(key);
}