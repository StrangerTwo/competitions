package com.example.module

import com.mongodb.ConnectionString
import com.mongodb.MongoClientSettings
import com.mongodb.kotlin.client.coroutine.MongoClient
import com.mongodb.kotlin.client.coroutine.MongoDatabase
import io.ktor.server.application.*
import org.bson.codecs.configuration.CodecRegistries
import org.bson.codecs.configuration.CodecRegistry
import org.bson.codecs.kotlinx.BsonConfiguration
import org.bson.codecs.kotlinx.KotlinSerializerCodec


@PublishedApi
internal lateinit var mongoClient: MongoClient;

fun Application.configureDatabases() {
    mongoClient = connectToMongoDB()
}

class DatabaseContext(val database: MongoDatabase)

inline fun <T : Any> withDatabase(databaseName: String, body: DatabaseContext.() -> T): T {
    val database = mongoClient.getDatabase(databaseName);
    return body(DatabaseContext(database = database))
}

/**
 * Establishes connection with a MongoDB database.
 *
 * The following configuration properties (in application.yaml/application.conf) can be specified:
 * * `db.mongo.user` username for your database
 * * `db.mongo.password` password for the user
 * * `db.mongo.host` host that will be used for the database connection
 * * `db.mongo.port` port that will be used for the database connection
 * * `db.mongo.maxPoolSize` maximum number of connections to a MongoDB server
 * * `db.mongo.database.name` name of the database
 *
 * IMPORTANT NOTE: in order to make MongoDB connection working, you have to start a MongoDB server first.
 * See the instructions here: https://www.mongodb.com/docs/manual/administration/install-community/
 * all the parameters above
 *
 * @returns [MongoDatabase] instance
 * */
fun Application.connectToMongoDB(): MongoClient {
    val user = System.getenv("DB_MONGO_USER")
    val password = System.getenv("DB_MONGO_PASSWORD")
    val host = System.getenv("DB_MONGO_HOST") ?: "127.0.0.1"
    val port = System.getenv("DB_MONGO_PORT") ?: "27017"
    val maxPoolSize = System.getenv("DB_MONGO_MAX_POOL_SIZE")?.toInt() ?: 20

    val credentials = user?.let { userVal -> password?.let { passwordVal -> "$userVal:$passwordVal@" } }.orEmpty()
    val uri = "mongodb://$credentials$host:$port/?maxPoolSize=$maxPoolSize&w=majority"

    val mongoClient = MongoClient.create(
        settings = MongoClientSettings
            .builder()
            .applyConnectionString(ConnectionString(uri))
//            .codecRegistry(codecRegistry())
            .build()
    )

    environment.monitor.subscribe(ApplicationStopped) {
        mongoClient.close()
    }

    return mongoClient;
}