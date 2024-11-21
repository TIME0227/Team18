# <img src="https://file.notion.so/f/f/814ce92c-f43f-4be5-b3c1-f67cf319bf48/c76f84d9-fd43-4733-8e7e-ddf5bfb4a88a/%EC%9A%B0%EC%A3%BC%ED%83%80%EC%9D%B4%EA%B1%B0_%EB%A1%9C%EA%B3%A0_black.png?id=5ae38cae-840c-4acd-bcf8-094ae7c5ea58&table=block&spaceId=814ce92c-f43f-4be5-b3c1-f67cf319bf48&expirationTimestamp=1718618400000&signature=BInsqRGl4ELkK69N2tcmCHO4nO0W11u9UhhPwXG2M6k&downloadName=%EC%9A%B0%EC%A3%BC%ED%83%80%EC%9D%B4%EA%B1%B0_%EB%A1%9C%EA%B3%A0_black.png" width="35"> 캡스톤디자인과창업프로젝트 18팀 우주타이거
### 주관적인 심리적 불편감을 겪는 20-30대를 위한 LLM기반 개인 맞춤형 챗봇 상담 서비스

### 👩‍👧‍👧 팀원
이다인(팀장), 강예진, 팽지원
### 📆 기간
24.03.04 ~ 24.12.

### 📌 프로젝트 소개
+ ❔ 타겟 고객: 주관적인 심리적 불편감을 겪는 20-30대
+ ❗ 해결 방안: 
  + **맥락에 맞춘 그래픽 기반 반응**<br>: NPC의 표정과 모션을 통해 정서적 연결감을 제공하여, 텍스트만으로는 느끼기 어려운 친밀감과 몰입감을 제공.
  + **맞춤형 피드백 리포트**<br>: 사용자의 대화 기록을 분석해, 개인 맞춤형 리포트를 능동적으로 제공.
  + **상담 목적으로 설계된 다양한 캐릭터**<br>: 다양한 성격과 상담 이론에 맞춘 캐릭터들이 제공되어, 사용자가 자신의 필요에 맞는 상담사 선택 가능.
  + **이전 대화를 기억하는 지속성**<br>: 이전 대화를 기억해 연속적인 상담을 가능하게 하여, 사용자에게 더 일관성 있는 상담 경험을 제공.

### 😊 상담사 리스트
1. **인지 치료 상담사**
  + 불안과 우울에 효과적이라고 알려져 있어요
  + 우리가 불안하고 우울한 이유는 부정적이고 극단적인 사고 때문일 확률이 커요. 부정적 사고 대신  유연한 사고를 같이 찾아나가는 과정입니다.


2. **장점찾기 상담사**
  + 함께 대화를 나누며 숨겨진 장점을 발견하고, 자신을 더 긍정적으로 바라볼 수 있도록 도와줍니다.


3. **상냥한 친구**
  + 당신만을 생각하고 위하는 순수하고 상냥한 친구입니다.
  + 일상적인 얘기와 고민을 나눠보세요.


4. **현실적이고 시니컬한 상담사**
  + MBTI가 T 유형인 분들에게 추천드려요
  + 현실적이고 맞는 말만 하는 조언을 듣고 싶다면 대화해보세요.

### ✏️ 사용 기술
<a href="https://unity.com/kr" target="_blank"><img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white"/></a><br>
<a href="https://openai.com/index/hello-gpt-4o/" target="_blank"><img src="https://img.shields.io/badge/OpenAI-412991?style=for-the-badge&logo=openai&logoColor=white"/></a> gpt-4o (LLM)<br>
<a href="https://sqlite.org/" target="_blank"><img src="https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white"/></a>

**Server** [리포지토리 바로가기](https://github.com/yjk395/Team18-Server) <br>
<img src="https://img.shields.io/badge/node.js-6DA55F?style=for-the-badge&logo=node.js&logoColor=white"/></a>
<img src="https://img.shields.io/badge/Vercel-000000?style=for-the-badge&logo=vercel&logoColor=white"/></a>
<img src="https://img.shields.io/badge/OpenAI%20API-eee?style=for-the-badge&logo=openai&logoColor=412991"/></a>

### 📝 기술검증
1. Unity에 chatGPT API 연결
2. 4가지 스타일의 상담사 프롬프트 작성 & 테스트
   + 체험 : https://team18-nnpkyfvownh9obzlnmdztd.streamlit.app/
   + 시연 영상 : https://youtu.be/2KhGn7ZIvBc
3. 상담사의 감정표현을 선택하는 프롬프트 작성 & 테스트
4. 요약을 통한 이전 대화 기억 프롬프트 작성
