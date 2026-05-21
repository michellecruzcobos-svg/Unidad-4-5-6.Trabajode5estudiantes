#include <Adafruit_Sensor.h>

#include <Adafruit_Sensor.h>

#include "DHT.h"

#define DHTPIN 6
#define DHTTYPE DHT11 

DHT dht(DHTPIN, DHTTYPE);

void setup() {
  Serial.begin(9600);
dht.begin();

}

void loop(){
  delay(2000);

  float h=dht.readHumidity();
  float t=dht.readTemperature();
  
  if(isnan(h) || isnan(t)){
    Serial.println("Error al leer del sensor DHT11");
    return;
  }

  Serial.print("Humedad: ");
  Serial.print(h);
  Serial.print("%\t");
  Serial.print("Temperatura: ");
  Serial.print(t);
  Serial.print("*C");
}