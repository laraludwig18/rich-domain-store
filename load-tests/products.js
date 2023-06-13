import http from 'k6/http'
import { Counter } from 'k6/metrics'
import { check } from 'k6'

// config
const config = {
    BASE_URL: __ENV.BASE_URL || 'http://localhost:8000',
}

// checks
const checks = response => {
    check(response, {
        isJSON: r => r.headers['Content-Type'] && r.headers['Content-Type'].startsWith('application/json'),
        is200: r => r.status === 200,
    })
    return response
}

// metrics
const ServerError = new Counter('server_errors')

const report = response => {
    if (response.status >= 500 && response.status < 600) {
        ServerError.add(1)
    }
    return response
}

// options
export const options = {
    stages: [
        { duration: '1s', target: 3000 },
        { duration: '1m', target: 5000 },
    ],
    thresholds: {
        server_errors: ['count<=5'],
        http_req_duration: ['p(95)<=300'],
        http_req_duration: ['p(99)<=500'],
        http_req_failed: ['rate<=0.05'],
    },
}


// executions
export default () => {
    const response = http.get(`${config.BASE_URL}/api/v1/products`)
    checks(report(response))
}
