const fs = require('fs');
const path = require('path');

const pkgPath = path.resolve(__dirname, '..', 'package.json');
const readmePath = path.resolve(__dirname, '..', 'README.md');

if (!fs.existsSync(pkgPath)) {
  console.error('package.json not found at', pkgPath);
  process.exit(1);
}

const pkg = JSON.parse(fs.readFileSync(pkgPath, 'utf8'));
const version = pkg.version;
if (!version) {
  console.error('Version not found in package.json');
  process.exit(1);
}

if (!fs.existsSync(readmePath)) {
  console.error('README.md not found at', readmePath);
  process.exit(1);
}

const readme = fs.readFileSync(readmePath, 'utf8');

const newBadge = `![Version: ${version}](https://img.shields.io/badge/Version-${encodeURIComponent(version)}-lightgrey.svg)`;

const badgeRegex = /!\[Version:[^\]]*\]\(https:\/\/img\.shields\.io\/badge\/Version-[^\)]+\)/;

if (badgeRegex.test(readme)) {
  const newReadme = readme.replace(badgeRegex, newBadge);
  if (newReadme === readme) {
    console.log('README already up to date.');
    process.exit(0);
  } else {
    fs.writeFileSync(readmePath, newReadme, 'utf8');
    console.log(`Updated README version badge to ${version}`);
    process.exit(0);
  }
} else {
  const lines = readme.split(/\r?\n/);
  let insertAt = 0;
  for (let i = 0; i < lines.length; i++) {
    if (lines[i].trim().startsWith('#')) {
      insertAt = i + 1;
      break;
    }
  }
  lines.splice(insertAt, 0, newBadge);
  fs.writeFileSync(readmePath, lines.join('\n'), 'utf8');
  console.log('Inserted version badge into README.');
  process.exit(0);
}

