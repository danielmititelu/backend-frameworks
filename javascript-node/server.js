/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

'use strict';

const express = require('express');
const {
	v4: uuidv4,
} = require('uuid');

// Constants
const PORT = 5000;
const HOST = '0.0.0.0';

let books = new Map();
// App
const app = express();
app.use(express.json());

app.get('/', (req, res) => {
	res.send('Hello world!\n');
});

app.get('/api/book', (req, res) => {
	res.send([...books.values()]);
});

app.post('/api/book', (req, res) => {
	if (validateBody(req.body)) {
		res.send(400, "Invalid request");
	}

	const guid = uuidv4();
	var book = {
		id: guid,
		title: req.body.title,
		author: req.body.author,
		pages: req.body.pages
	};

	books.set(guid, book);
	res.status(201).send(book);
});

app.delete('/api/book/:id', (req, res) => {
	if (req.params.id === undefined) {
		res.send(400, "Invalid request");
	}

	books.delete(req.params.id);
	return res.status(204).send();
});

app.get('/api/book/:id', (req, res) => {
	if (req.params.id === undefined) {
		res.send(400, "Invalid request");
	}
	
	return res.status(200).send(books.get(req.params.id));
});

app.listen(PORT, HOST);
console.log(`Running on http://${HOST}:${PORT}`);

function validateBody(req) {
	return req === null ||
		["title", "author", "pages"].every(prop =>
			!Object.prototype.hasOwnProperty.call(req, prop)
		);
}
