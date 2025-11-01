import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('http://localhost:4200/');
  await expect(page).toHaveTitle("GraphqlChat");
});

test('Type message', async ({ page }) => {
  await page.goto('http://localhost:4200/');
  await page.getByTestId('addMessageInput').fill('ugh')
  await page.getByTestId('sendMessageBtn').click();

  const messages = page.getByTestId("chatMessages").allInnerTexts();
  await expect((await messages).some(message => message.includes('ugh'))).toBeTruthy()
});
