// === CONFIGURACIÓN DEL DISCO ===
let avgHue = 0
let strip: neopixel.Strip = null
let acc = 0
let lights: Light[] = []
let ringStart: number[] = []

let RINGS = [
    60,
    48,
    40,
    32,
    24,
    16,
    12,
    8,
    1
] // centro a exterior

const TOTAL_PIXELS = RINGS.reduce((a, b) => a + b, 0)
let centerIntensity = 0;

for (let count of RINGS) {
    ringStart.push(acc)
    acc += count
}

// === VARIABLES GLOBALES ===
strip = neopixel.create(DigitalPin.P0, TOTAL_PIXELS, NeoPixelMode.RGB)

class Light {
    ring: number
    index: number
    hue: number
    prevIndex: number
    prevRing: number

    constructor() {
        this.ring = 0 // Centro
        this.hue = Math.randomRange(0, 359)
        this.index = Math.floor(this.hue / 6)
        this.prevIndex = 0
        this.prevRing = 0
    }

    isAlive(): boolean {
        return this.ring >= 0 && this.ring < RINGS.length
    }

    step() {
        let move = Math.randomRange(8 - this.ring, 10) // 0: out, 1: right, 2: left
        let ringSize = RINGS[this.ring]
        this.prevIndex = this.index
        this.prevRing = this.ring

        if (move < 9) {
            let nextRing = this.ring + 1
            let nextSize = RINGS[nextRing]
            this.index = Math.round(this.index * nextSize / ringSize) % nextSize
            this.ring = nextRing
        } else if (move == 9) {
            this.index = (this.index + 1) % ringSize
        } else if (move == 10) {
            this.index = (this.index - 1 + ringSize) % ringSize
        }
    }

    draw(buffer: number[][]) {
        if (!this.isAlive()) return
        let ringSize = RINGS[this.ring]
        let prevRingSize= RINGS[this.prevRing]

        let pos = ringStart[this.ring] + this.index
        if (!buffer[pos]) buffer[pos] = []
        buffer[pos].push(this.hue)

        pos = ringStart[this.ring] + (this.index - 1 + ringSize) % ringSize
        if (!buffer[pos]) buffer[pos] = []
        buffer[pos].push(this.hue)

        pos = ringStart[this.ring] + (this.index + 1) % ringSize
        if (!buffer[pos]) buffer[pos] = []
        buffer[pos].push(this.hue)
        

        pos = ringStart[this.prevRing] + this.prevIndex
        if (!buffer[pos]) buffer[pos] = []
        buffer[pos].push(this.hue)
        pos = ringStart[this.prevRing] + (this.prevIndex - 1 + prevRingSize) % prevRingSize
        if (!buffer[pos]) buffer[pos] = []
        buffer[pos].push(this.hue)
        pos = ringStart[this.prevRing] + (this.prevIndex + 1) % prevRingSize
        if (!buffer[pos]) buffer[pos] = []
        buffer[pos].push(this.hue)
    }
}

function drawLightBuffer(buffer: number[][]) {

    let ringintesities = getLogScaledRingIntensities()

    for (let i = 0; i < TOTAL_PIXELS; i++) {
        if (!buffer[i] || buffer[i].length == 0) {
            strip.setPixelColor(i, neopixel.colors(NeoPixelColors.Black))
        } else {
            avgHue = buffer[i].reduce((a, b) => a + b, 0) / buffer[i].length
            let intensity =  5 * buffer[i].length;
            let ring =  getRingFromIndex(i);
            strip.setPixelColor(i, neopixel.hsl(avgHue, 99, intensity + ringintesities[ring]))
        }
    }

    
    strip.show()
}
function getRingFromIndex(index: number): number {
    for (let ring = 0; ring < ringStart.length; ring++) {
        let start = ringStart[ring]
        let count = RINGS[ring]
        if (index >= start && index < start + count) {
            return ring
        }
    }
    return -1 // Si el índice está fuera del rango
}
function getLogScaledRingIntensities(): number[] {
    let result: number[] = []
    let maxRing = RINGS.length - 1
    for (let i = 0; i <= maxRing; i++) {
        // Apply logarithmic decay (log(1 + x) for better scaling)
        let factor = Math.log(1 + (i)) / Math.log(1 + maxRing)
        result.push(Math.idiv(centerIntensity * factor, 1))
    }
    return result
}

loops.everyInterval(150, function () {
    lights.push(new Light())
})

loops.everyInterval(150, function () {
    let buffer: number[][] = []
    
    for (let light of lights) {
        light.step()
        light.draw(buffer)
    }
    let lightsCount = lights.length;
    lights = lights.filter(l => l.isAlive())
    centerIntensity += lightsCount-lights.length;
    drawLightBuffer(buffer)
})