# Eyeshot Report

## 1. Issue - Loading an entity takes an enormous amount of time and allocates GBs of memory.

[project](https://github.com/adeko2/EyeshotRreport/tree/master/EyeshotReport)
We have observed an abnormal behaviour during loading an entity produced from a dwg source. 
- I seems to stuck in an infinate loop inside `design1.Entities.Add(blockReference)`
- The app continues to allocate memory with a high speed without slowing down. During our test case we reached out to 14 GB of memory.
![image](https://github.com/user-attachments/assets/86456c87-1f48-4ca4-bc33-850154e94445)
