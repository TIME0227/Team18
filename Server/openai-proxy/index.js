const express = require('express');
const axios = require('axios');
const bodyParser = require('body-parser');
const cors = require('cors');
const { Configuration, OpenAIApi } = require('openai');require('dotenv').config(); // OpenAI API 키 환경 변수 로드

const app = express();
const PORT = process.env.PORT || 3000;
//const OPENAI_API_KEY = process.env.OPENAI_API_KEY; // OpenAI API 키

const configuration  = new Configuration({
    api_key: process.env.OPENAI_API_KEY
});

const openai = new OpenAIApi(configuration);

// 미들웨어 설정
app.use(cors()); // CORS 설정: 모든 도메인 허용 (배포 시 특정 도메인만 허용하는 것이 안전함. 예: Unity WebGL 빌드의 도메인)
app.use(bodyParser.json()); // JSON 파싱 설정

// ChatGPT에 요청을 보내는 함수
async function getChatGPTResponse(messages) {
    try {
        const response = await openai.createChatCompletion({
            model: 'gpt-4o', // 기본 모델 설정
            messages: messages,
            max_tokens: 1000, // 기본 토큰 설정
          });

        // 모델 응답에서 답변을 가져옴
        return response.choices[0].message.content;    
    } catch (error) {
        console.error('OpenAI API 요청 오류:', error.message);
        throw error;
    }
}

// 프록시 엔드포인트 정의
app.post('/chatgpt', async (req, res) => {
    try {
        const response = await getChatGPTResponse(req.body.messages);

        // OpenAI 응답을 클라이언트로 전달
        res.json({response});
    } catch (error) {
        console.error(error.response ? error.response.data : error.message);
        res.status(500).send('Server Error');
    }
});

// 서버 실행
app.listen(PORT, () => {
    console.log(`Proxy server running on http://localhost:${PORT}`);
});