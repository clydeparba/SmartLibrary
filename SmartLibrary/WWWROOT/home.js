async function executeRequest(endpointId, method, path, hasBody, params, event) {
    const btn = event.target;
    btn.disabled = true;
    btn.textContent = 'Loading...';
    let url = path;
    let queryParams = [];
    params.forEach(function (param) {
        const value = document.getElementById('param-' + endpointId + '-' + param).value;
        if (path.includes('{' + param + '}')) {
            url = url.replace('{' + param + '}', value);
        } else {
            queryParams.push(param + '=' + encodeURIComponent(value));
        }
    });
    if (queryParams.length > 0) {
        url += '?' + queryParams.join('&');
    }
    const options = { method: method, headers: { 'Content-Type': 'application/json' } };
    if (hasBody) {
        const bodyText = document.getElementById('body-' + endpointId).value;
        try {
            JSON.parse(bodyText);
            options.body = bodyText;
        } catch (e) {
            showResponse(endpointId, 400, 'Invalid JSON in request body');
            btn.disabled = false;
            btn.textContent = 'Try it out';
            return;
        }
    }
    try {
        const response = await fetch(url, options);
        const contentType = response.headers.get("content-type");
        let responseData;
        if (contentType && contentType.includes("application/json")) {
            responseData = await response.json();
        } else {
            responseData = await response.text();
        }
        showResponse(endpointId, response.status, responseData);
    } catch (error) {
        showResponse(endpointId, 0, 'Error: ' + error.message);
    }
    btn.disabled = false;
    btn.textContent = 'Try it out';
}