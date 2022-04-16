<template>
  <div class="notfound">
    <div class="background"></div>
    <div class="message">
      <h1>404</h1>
      <h2>I wish it was 418 but it is just regular 404</h2>
    </div>
  </div>
</template>

<style>
.background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: url(../assets/404bg.webp) 50% no-repeat;
  background-size: cover;
  z-index: -1;
  opacity: 0.5;
}
.notfound {
  position: absolute;
  top: 0;
  left: 0;
  z-index: -1;
  width: 100%;
  height: 100%;
  background-color: black;
}
.message {
  -webkit-font-smoothing: antialiased;
  color: #fff;
  position: absolute;
  width: 100%;
  top: 50%;
  transform: translateY(-50%);
}

.message h1 {
  font-size: 144px;
  font-weight: 100;
  margin-bottom: 16px;
}
</style>