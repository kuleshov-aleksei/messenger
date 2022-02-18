import http from "k6/http";
import { check } from 'k6';

export let options = {
    vus: 1,
    stages: [
        { duration: "1m", target: 10 }, 
        { duration: "3m", target: 50 },
        { duration: "1m", target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(99)<150'], // 99% of all requests must complete below 150ms
    },
}

export function setup() {
    // register a new user and authenticate via a Bearer token.
    const res = http.post("http://127.0.0.1:23581/auth/auth", JSON.stringify({
        login: "vlad_i_slav@example.com",
        password: "my_password",
        device_name: "k6s",
    }));

    const authToken = res.json("access_token");
    check(authToken, { "logged in successfully": () => authToken !== "" });
    check(res, {
        'is status 200': (r) => r.status === 200,
    });

  return authToken;
}

export default (authToken) => {
    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authToken}`,
        }
    };

    const payload = JSON.stringify({
        chat_id: 1,
    });
    const res = http.post("http://localhost:5000/messenger/historical/get_last_messages", payload, params);

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}