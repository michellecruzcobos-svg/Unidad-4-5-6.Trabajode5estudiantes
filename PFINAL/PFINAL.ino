#include <Arduino.h>
#include <Servo.h>

// --- PINES ---
const int ledRojo     = 2;
const int ledAmarillo = 3;
const int ledAzul     = 4;
const int ledVerde    = 5;
const int relevador   = 6;
const int sensorHumedad = 7;
const int sensorAgua  = 8;
const int buzzer      = 9;
const int pinServo    = 10;
const int pinEcho     = 11;
const int pinTrig     = 12;
const int buzzer2     = 13;
const int ledAlertaA0 = A0;

// --- VARIABLES ---
bool estadoRojo = false, estadoAmarillo = false;
bool estadoAzul = false, estadoVerde = false, estadoRelevador = false;
long duracion;
int distancia;
int anguloActual = 0;
unsigned long tiempoAnteriorLed    = 0;
unsigned long tiempoAnteriorSerial = 0;
bool estadoLedAlerta = false;

Servo miServo;

void setup() {
  pinMode(ledRojo,      OUTPUT);
  pinMode(ledAmarillo,  OUTPUT);
  pinMode(ledAzul,      OUTPUT);
  pinMode(ledVerde,     OUTPUT);
  pinMode(relevador,    OUTPUT);
  pinMode(buzzer,       OUTPUT);
  pinMode(buzzer2,      OUTPUT);
  pinMode(ledAlertaA0,  OUTPUT);
  pinMode(pinTrig,      OUTPUT);
  pinMode(pinEcho,      INPUT);
  pinMode(sensorHumedad, INPUT);
  pinMode(sensorAgua,   INPUT);

  miServo.attach(pinServo);
  miServo.write(90);
  Serial.begin(9600);
}

void loop() {
  for (int i = 0; i <= 180; i++) { moverSoloServo(i); }
  for (int i = 180; i >= 0; i--) { moverSoloServo(i); }
}

void moverSoloServo(int angulo) {
  anguloActual = angulo;
  miServo.write(angulo);
  procesarSensoresYTeclado();
  delay(20);   // ✅ Se mantiene igual que tu código original
}

void procesarSensoresYTeclado() {
  // --- ULTRASÓNICO ---
  digitalWrite(pinTrig, LOW);
  delayMicroseconds(2);
  digitalWrite(pinTrig, HIGH);
  delayMicroseconds(10);
  digitalWrite(pinTrig, LOW);
  duracion  = pulseIn(pinEcho, HIGH);
  distancia = duracion * 0.034 / 2;

  // Buzzer proximidad
  if (distancia > 0 && distancia < 20) {
    if ((millis() / 200) % 2 == 0) tone(buzzer2, 1200);
    else noTone(buzzer2);

    if (millis() - tiempoAnteriorLed >= 200) {
      tiempoAnteriorLed = millis();
      estadoLedAlerta   = !estadoLedAlerta;
      digitalWrite(ledAlertaA0, estadoLedAlerta);
    }
  } else {
    noTone(buzzer2);
    digitalWrite(ledAlertaA0, LOW);
  }

  // --- SENSOR AGUA ---
  int lecturaAgua = digitalRead(sensorAgua);
  if (lecturaAgua == HIGH) {
    if ((millis() / 300) % 2 == 0) tone(buzzer, 900);
    else tone(buzzer, 600);
  } else {
    noTone(buzzer);
  }

  // --- ENVÍO SERIAL CON FORMATO FIJO (cada 300 ms) ---
  if (millis() - tiempoAnteriorSerial >= 300) {
    tiempoAnteriorSerial = millis();

    int lecturaHumedad = digitalRead(sensorHumedad);

    // Ángulo y distancia en una sola línea (para el radar en C#)
    Serial.print("ANG:");
    Serial.println(anguloActual);

    Serial.print("DIST:");
    Serial.println(distancia);

    // Humedad digital (SECO / HUMEDO)
    Serial.print("HUM:");
    Serial.println(lecturaHumedad == HIGH ? "SECO" : "HUMEDO");

    // Agua
    Serial.print("AGUA:");
    Serial.println(lecturaAgua == HIGH ? "ALERTA" : "OK");

    // Estado relevador
    Serial.print("RELAY:");
    Serial.println(estadoRelevador ? "1" : "0");
  }

  // --- CONTROL POR TECLADO ---
  if (Serial.available() > 0) {
    char tecla = Serial.read();
    switch (tecla) {
      case 'e': case 'E':
        estadoRojo = !estadoRojo;
        digitalWrite(ledRojo, estadoRojo);
        break;
      case 'r': case 'R':
        estadoAmarillo = !estadoAmarillo;
        digitalWrite(ledAmarillo, estadoAmarillo);
        break;
      case 't': case 'T':
        estadoAzul = !estadoAzul;
        digitalWrite(ledAzul, estadoAzul);
        break;
      case 'y': case 'Y':
        estadoVerde = !estadoVerde;
        digitalWrite(ledVerde, estadoVerde);
        break;
      case 'u': case 'U':
        estadoRelevador = !estadoRelevador;
        digitalWrite(relevador, estadoRelevador);
        // Confirmar estado inmediatamente
        Serial.print("RELAY:");
        Serial.println(estadoRelevador ? "1" : "0");
        break;
    }
  }
}