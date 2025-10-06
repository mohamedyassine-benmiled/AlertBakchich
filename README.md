# AlertBakchich

A self-hosted, highly customizable alert system for Ba9chich donations, designed for streamers. This project allows you to receive webhook notifications from Ba9chich and display animated alerts in OBS or any browser source.

---

## Features
- Receive and display Ba9chich donation alerts in real time
- Fully customizable alert appearance (media, text, colors, animations, etc.)
- Live configuration and preview page
- Supports video, image, and GIF media
- Queueing and sound support

---

## 1. Configuration

1. **Open the configuration page**
   - Go to `http://localhost:5000/config` (or your tunnel/public URL + `/config`)

2. **Customize your alert**
   - Set media (image, video, or GIF URL)
   - Adjust text, colors, fonts, border, background, and animation
   - Advanced: Set z-index, text position, text width, custom CSS/JS, etc.
   - Use the live preview to test your alert

3. **Save your configuration**
   - Click "Save Configuration". **Note:** You must refresh the alert page in OBS or your browser after every configuration save for changes to take effect.

---

## 2. Hosting the Application

### Localhost (for local testing)
- Run the app:
  ```sh
  dotnet run --urls=http://0.0.0.0:5000
  ```
- Access the config at `http://localhost:5000/config`
- Access the alert at `http://localhost:5000/alert`

### Exposing to the Internet (for Ba9chich webhooks)

#### Option 1: Cloudflare Tunnel
- [Install Cloudflare Tunnel](https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/install-and-setup/installation/)
- Start a tunnel:
  ```sh
  cloudflared tunnel --url http://localhost:5000
  ```
- Copy the public URL from the output (e.g., `https://yourname.trycloudflare.com`)

#### Option 2: Ngrok
- [Install Ngrok](https://ngrok.com/download)
- Start a tunnel:
  ```sh
  ngrok http 5000
  ```
- Copy the public URL from the output (e.g., `https://xxxx.ngrok.io`)

#### Option 3: Port Forwarding
- Forward port 5000 on your router to your PC
- Use your public IP: `http://your-public-ip:5000`

---

## 3. Connecting Ba9chich to Your Server

1. **Get your webhook URL**
   - If using a tunnel, use the public URL (e.g., `https://xxxx.ngrok.io/webhook`)
   - If using port forwarding, use your public IP (e.g., `http://your-public-ip:5000/webhook`)

2. **Set the webhook in Ba9chich**
   - Go to [https://ba9chich.com/dashboard/en/profile?tab=settings](https://ba9chich.com/dashboard/en/profile?tab=settings)
   - Scroll to the bottom of the page
   - Paste your webhook URL in the appropriate field

---

## 4. Setting Up the Alert in OBS

1. **Get your alert display URL**
   - Use your tunnel/public URL + `/alert` (e.g., `https://xxxx.ngrok.io/alert`)
   - Or, for local testing: `http://localhost:5000/alert`

2. **Add a browser source in OBS**
   - URL: your alert display URL
   - Set the width/height to match your stream resolution (e.g., 1280x720)

3. **Test your alert**
   - Use the preview/test button in the config page
   - Send a test donation from Ba9chich

---

## PS
- **You must refresh the alert page in OBS or your browser after every configuration save for changes to take effect.**
- If you change your tunnel/public URL, update it in both Ba9chich and OBS.

---

## Troubleshooting
- If alerts do not show up, check your tunnel/public URL and webhook settings.
- Make sure your server is running and accessible from the internet.
- Check the console/log output for errors.

---

## Security Warning: Webhook Verification

> **Warning:** Ba9chich does not provide a verification system for webhook calls. This means that the `/webhook` endpoint in this application will accept any POST request sent to it, regardless of the sender. There is no way to verify that a webhook call actually comes from Ba9chich.
>
> - **Do not share your webhook URL publicly.**
> - Anyone who knows your webhook URL can trigger fake alerts.
> - If you need stronger security, consider restricting access to the webhook endpoint (e.g., with a firewall, IP allowlist, or reverse proxy authentication).
